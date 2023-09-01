namespace UnityLib.Architecture.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityLib.Architecture.Di.CancellationFactory;
    using UnityLib.Architecture.Log;

    /// <summary>
    /// Инжектор зависимостей.
    /// </summary>
    public static class Injector
    {
        /// <summary>
        /// Словарь всех объектов.
        /// </summary>
        private static readonly Dictionary<Type, object> _dictionary;

        /// <summary>
        /// Словарь привязок, которые должны быть выполнены после создания объекта.
        /// </summary>
        /// <remarks> Пока что не понятно нужно ли это; где-то еще. РАБОТАЕТ только для синглтонов игры. </remarks>
        private static readonly Dictionary<Type, List<Action<object>>> _dictionaryAfterBindings;

        /// <summary>
        /// Словарь всех объектов на сцене.
        /// </summary>
        private static readonly Dictionary<Type, object> _dictionaryScene;

        /// <summary>
        /// Словарь с типами которые нужно инициализировать.
        /// <c>
        /// Ключ: тип объекта; Значение: является объектом одной сцены,
        /// необходимые типы параметров.
        /// </c>
        /// </summary>
        private static readonly List<PickyInstance> _pickyInstances;

        /// <summary>
        /// Инжектор зависимостей.
        /// </summary>
        static Injector()
        {
            _dictionary = new Dictionary<Type, object>();
            _dictionaryScene = new Dictionary<Type, object>();
            _dictionaryAfterBindings = new Dictionary<Type, List<Action<object>>>();
            _pickyInstances = new List<PickyInstance>();
        }

        /// <summary>
        /// Применить привязку.
        /// </summary>
        /// <typeparam name="TType"> Тип. </typeparam>
        /// <param name="obj"> Объект. </param>
        /// <remarks> После применения записи о действия удаляются (которые были раннее). </remarks>
        public static void ApplyBind<TType>(TType obj)
        {
            var type = typeof(TType);
            if (!_dictionaryAfterBindings.TryGetValue(type, out var list))
                return;

            list.ForEach(action => action.Invoke(obj));
            _dictionaryAfterBindings.Remove(type);
        }

        /// <summary>
        /// Забиндить некоторые действия, которые нужно сделать после создания объекта.
        /// </summary>
        /// <typeparam name="TType"> Тип. </typeparam>
        /// <param name="action"> Действие. </param>
        /// <remarks> Если объект уже существует действия применяться сразу. </remarks>
        public static void BindAfterAction<TType>(Action<TType> action)
        {
            var type = typeof(TType);
            if (_dictionary.TryGetValue(type, out var obj))
            {
                action((TType)obj);
                return;
            }

            if (!_dictionaryAfterBindings.TryGetValue(type, out var afterActionList))
            {
                afterActionList = new List<Action<object>>();
                _dictionaryAfterBindings.Add(type, afterActionList);
            }

            afterActionList.Add(singleton => action((TType)singleton));
        }

        /// <summary>
        /// All classes from local map Unity will delete and injector clear dictionary with them.
        /// </summary>
        public static void ClearSceneInstances()
        {
            // Обрабатываем уничтожение объектов, которые как-то связаны еще.
            foreach (var pair in _dictionaryScene)
            {
                if (!(pair.Value is IProcessDestruction processDestruction))
                    continue;

                processDestruction.ProcessDestruction();
            }

            // Обновляем в фабрике, изменяемые токены.
            Get<CancellationTokenFactory>().CancelSceneTokens();

            GameLogger.Info($"Сброс синглтонов сцены, в Injector-е. Количество: {_dictionaryScene.Count}.");

            // Очищаем словарь от одиночек сцены.
            foreach (var pair in _dictionaryScene)
            {
                if (pair.Value is IDisposable disposable)
                    disposable.Dispose();
            }

            _dictionaryScene.Clear();

            var scenePickyInstances = _pickyInstances.Where(p => p.ExistsOnScene).ToList();
            scenePickyInstances.ForEach(p => _pickyInstances.Remove(p));

            GC.Collect();
        }

        /// <summary>
        /// Существует элемент в словаре.
        /// </summary>
        /// <typeparam name="T"> Тип элемента. </typeparam>
        /// <returns> True если существует. </returns>
        public static bool Exists<T>()
        {
            return Exists(typeof(T));
        }

        /// <inheritdoc cref="Exists{T}" />
        public static bool Exists(Type type)
        {
            return _dictionary.TryGetValue(type, out _);
        }

        /// <summary>
        /// Существует игровой элемент сцены, в словаре объектов сцены.
        /// </summary>
        /// <typeparam name="T"> Тип элемента. </typeparam>
        /// <returns> True если существует. </returns>
        public static bool ExistsSceneObject<T>()
        {
            return ExistsSceneObject(typeof(T));
        }

        /// <inheritdoc cref="Exists{T}" />
        public static bool ExistsSceneObject(Type type)
        {
            return _dictionaryScene.TryGetValue(type, out _);
        }

        /// <summary>
        /// Получить зарегистрированный объект.
        /// </summary>
        /// <typeparam name="T"> Тип объекта; интерфейс. </typeparam>
        /// <returns> Объект. Вернет null если не будет объекта. </returns>
        public static T Get<T>() where T : class
        {
            if (_dictionary.TryGetValue(typeof(T), out var dictValue))
                return (T)dictValue;

            ProcessNotExistsObject<T>(false);
            return null;
        }

        /// <summary>
        /// Получить зарегистрированный объект на сцене.
        /// </summary>
        /// <typeparam name="T"> Тип объекта; интерфейс. </typeparam>
        /// <returns> Объект. Вернет null если не будет объекта. </returns>
        public static T GetSceneObject<T>()
            where T : class
        {
            if (_dictionaryScene.TryGetValue(typeof(T), out var dictSceneValue))
                return (T)dictSceneValue;

            ProcessNotExistsObject<T>(true);
            return null;
        }

        /// <summary>
        /// Зарегистрировать одиночку.
        /// </summary>
        /// <typeparam name="TSource"> Тип по которому будем запрашивать. </typeparam>
        /// <param name="source"> Что вернется. Если NULL создадим через Activator. </param>
        /// <param name="existsOnScene"> Существует одну сцену. </param>
        public static void RebindSingleton<TSource>(TSource source, bool existsOnScene)
            where TSource : class
        {
            if (source is null)
            {
                RebindSingleton<TSource, TSource>(existsOnScene);
                return;
            }

            if (ContainsTypeInDictionaries<TSource>(ref existsOnScene))
                throw new ErrorFoundException($"Зависимость типа {typeof(TSource).Name} уже зарегистрирована.");

            // Не удалять object, чтобы был контроль (ключа) типа абстракции.
            AddSourceInDictionaries(typeof(TSource), source, existsOnScene);
        }

        /// <summary>
        /// Зарегистрировать одиночку.
        /// </summary>
        /// <typeparam name="TSource"> Тип по которому будем запрашивать. </typeparam>
        /// <param name="existsOnScene"> Существует одну сцену. </param>
        public static void RebindSingleton<TSource>(bool existsOnScene)
        {
            RebindSingleton<TSource, TSource>(existsOnScene);
        }

        /// <summary>
        /// Зарегистрировать одиночку.
        /// </summary>
        /// <typeparam name="TInterface"> Тип по которому будем запрашивать. Интерфейс. </typeparam>
        /// <typeparam name="TSource"> Что вернется. </typeparam>
        /// <param name="existsOnScene"> Существует одну сцену. </param>
        public static void RebindSingleton<TInterface, TSource>(bool existsOnScene)
        {
            if (ContainsTypeInDictionaries<TInterface>(ref existsOnScene))
                throw new ErrorFoundException($"Зависимость типа {typeof(TSource).Name} уже зарегистрирована.");

            var interfaceType = typeof(TInterface);
            var sourceType = typeof(TSource);

            if (!HasHardConstruct(sourceType, out var parameters))
            {
                var source = Activator.CreateInstance<TSource>();
                AddSourceInDictionaries(interfaceType, source, existsOnScene);
                return;
            }

            var pickyInstance = _pickyInstances.FirstOrDefault(pi => pi.Type.FullName == sourceType.FullName);
            if (pickyInstance != null)
                throw new Exception($"Зависимость {pickyInstance.Type.Name} уже зарегистрирована");

            var dictionary = GetAllDependenciesDictionary();

            // TODO: ИТОГО. Нужно сделать так чтобы сразу создавался объект и добавлялся в словарь, а не проверялись зависимости.
            // ReSharper disable once UnusedVariable
            var hasDependencies = parameters.All(p => dictionary.Any(pair => pair.Key.FullName == p.FullName));

            var createdPickyInstance = new PickyInstance(existsOnScene, interfaceType, sourceType, parameters);
            _pickyInstances.Add(createdPickyInstance);
            CheckPickyTypes();
        }

        /// <summary>
        /// Удалить объект из словаря.
        /// </summary>
        /// <typeparam name="T"> Тип объекта. </typeparam>
        public static void Remove<T>()
        {
            _dictionary.Remove(typeof(T));
        }

        /// <summary>
        /// Удалить игровой объект из словаря сцены.
        /// </summary>
        /// <typeparam name="T"> Тип объекта. </typeparam>
        public static void RemoveSceneObject<T>()
        {
            _dictionaryScene.Remove(typeof(T));
        }

        /// <summary>
        /// Добавить объект в список.
        /// </summary>
        /// <param name="existsOnScene"> Существует одну сцену. </param>
        /// <param name="source"> Объект. </param>
        /// <param name="type"> Тип объекта. </param>
        private static void AddSourceInDictionaries(Type type, object source, bool existsOnScene)
        {
            var dictionary = GetDependenciesDictionary(existsOnScene);
            dictionary.Add(type, source);

            CheckPickyTypes();
        }

        /// <summary>
        /// Проверить требовательные типы с конструктарами, у которых есть параметры.
        /// </summary>
        private static void CheckPickyTypes()
        {
            var dictionary = GetAllDependenciesDictionary();
            var pickyInstances = _pickyInstances.ToList();

            var removedPickyInstance = new List<KeyValuePair<object, PickyInstance>>();
            foreach (var pickyInstance in pickyInstances)
            {
                // Получаем необходимые существующие синглтоны.
                var neededExistsInstances = dictionary.Where(
                    obj => pickyInstance.Parameters.Any(p => p.FullName == obj.Key.FullName)).ToArray();
                if (neededExistsInstances.Length != pickyInstance.Parameters.Length)
                    continue;

                var pairPickyInstance = CreatePickyInstance(pickyInstance, neededExistsInstances);
                removedPickyInstance.Add(pairPickyInstance);
            }

            // Создаем.
            foreach (var pairPickyInstance in removedPickyInstance)
            {
                var pickyInstance = pairPickyInstance.Value;
                AddSourceInDictionaries(pickyInstance.InterfaceType, pairPickyInstance.Key,
                    pickyInstance.ExistsOnScene);
            }
        }

        /// <summary>
        /// Проверить наличие типа в словарях.
        /// </summary>
        /// <typeparam name="T"> Тип. </typeparam>
        /// <param name="existsOnScene"> Проверять ли в словаре сцены. </param>
        /// <returns> TRUE - если есть. </returns>
        private static bool ContainsTypeInDictionaries<T>(ref bool existsOnScene)
        {
            return _dictionary.ContainsKey(typeof(T)) || (existsOnScene && _dictionaryScene.ContainsKey(typeof(T)));
        }

        /// <summary>
        /// Инициализировать инстанс, который создается с задержкой.
        /// </summary>
        /// <param name="pickyInstance"> Инстанс, который создается с задержкой. </param>
        /// <param name="neededExistsInstances"> Необходимые инстансы. </param>
        /// <returns> Ключ: созданный объект. Значение: описание инстанса с задержкой. </returns>
        private static KeyValuePair<object, PickyInstance> CreatePickyInstance(
            PickyInstance pickyInstance,
            KeyValuePair<Type, object>[] neededExistsInstances)
        {
            // Сортируем параметры в правильном порядке.
            var parameterInstances = new List<object>();
            var parameters = pickyInstance.Parameters;

            foreach (var parameter in parameters)
            {
                var parameterInstance = neededExistsInstances.First(eI => eI.Key.Name == parameter.Name);
                parameterInstances.Add(parameterInstance.Value);
            }

            var parameterInstancesArray = parameterInstances.ToArray();
            var source = Activator.CreateInstance(pickyInstance.Type, parameterInstancesArray);

            // Удаляем, иначе получим цикл; или еще того хуже создадим ветвленную рекурсию.
            _pickyInstances.Remove(pickyInstance);
            return new KeyValuePair<object, PickyInstance>(source, pickyInstance);
        }

        /// <summary>
        /// Получает словарь всех зависимостей.
        /// </summary>
        /// <returns> Словарь. </returns>
        private static Dictionary<Type, object> GetAllDependenciesDictionary()
        {
            return _dictionary.Concat(_dictionaryScene).ToDictionary(p => p.Key, p => p.Value);
        }

        /// <summary>
        /// Получает словарь зависимостей.
        /// </summary>
        /// <param name="existsOnScene"> Получить ли словарь сцены. </param>
        /// <returns> Словарь с зависимостями. </returns>
        private static Dictionary<Type, object> GetDependenciesDictionary(bool existsOnScene)
        {
            return existsOnScene ? _dictionaryScene : _dictionary;
        }

        /// <summary>
        /// Является ли тип с конструктором с параметрами.
        /// </summary>
        /// <param name="type"> Тип. </param>
        /// <param name="parameters"> Параметры в конструкторе. </param>
        /// <returns> TRUE - если тип сложный, имеет параметры в конструкторе. </returns>
        private static bool HasHardConstruct(Type type, out Type[] parameters)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length > 1)
                throw new NotSupportedException("Больше одного конструктора в синглтоне.");

            var constructor = constructors[0];
            parameters = constructor.GetParameters().Select(p => p.ParameterType).ToArray();

            return parameters.Length > 0;
        }

        /// <summary>
        /// Обрабатывает почему не существует объект.
        /// </summary>
        /// <param name="existsOnScene"> Существует на сцене. </param>
        /// <typeparam name="T"> Тип объекта. </typeparam>
        private static void ProcessNotExistsObject<T>(bool existsOnScene) where T : class
        {
            var pickyInstance = _pickyInstances.FirstOrDefault(pi => pi.InterfaceType == typeof(T));
            if (pickyInstance == null)
            {
                var word = existsOnScene ? "сцены " : string.Empty;
                GameLogger.Error($"В словаре объектов-одиночек {word}нет: {typeof(T).Name}");

                return;
            }

            var allDependencies = existsOnScene ? GetAllDependenciesDictionary() : _dictionary;
            var parameterNames = pickyInstance.Parameters.Where(p => !allDependencies.ContainsKey(p))
                .Select(pi => pi.Name);

            var scene = existsOnScene ? "на сцену, " : string.Empty;
            GameLogger.Error($"Добавлен одиночка в игру, {scene}но не создан: {typeof(T).Name}\n" +
                             $"Не достающие параметры: {string.Join(", ", parameterNames)}");
        }
    }
}