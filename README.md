# UnityLib
My unity lib with architecture solutions. 
Либа с архитектурными решениями для Unity. 


#### Постфиксы
- Controller - синглтон на сцене, удаляемый при перезагруке сцены.
- Manager - синглтон на сцене.
- Service - синглтон.
- Moderator - статичный класс с методами и полями.
- Utils - утилита.
- Worker - живет время выполнения операции.
- Ui - добавляем для объектов являющимися представлениями.

#### Префиксы
- Префикс B_<method_name> в названии метода использование метода кнопкой.
- Префикс E_<method_name> - обозначаем все методы вызываемые-указываемые из GUI.

#### Атрибуты
- SerializeField - используем для указания инициализации-заполнения, в Unity.

#### Наименования тегов TODO
* TODO - Что не обходимо сделать в рамках текущей задачи
* INFUT - Что сделать, если редактирование одного коснется этого участка кода
* ERROR - Была обнаружена ошибка.
* THINK - Мысли-задачи по task-manager.

#### Наименование тегов в Git
- FUTURE - добавил нововведение
- ERROR - исправил ошибку

#### Термины
Server - мастер-клиент, клиент, сервер.
Local - клиент.
Replica - копия объекта, управляемая другим игроком.

## ФИЧИ

#### PHOTON
При построении архитектуры, Client и изменяемый MasterClient.

При разделении логики, стоит использовать два типа воркеров, которые буду работать на двух сторонах;
Воркеры должны подключаться-отключаться после смены статуса игрока;
(Вариант работы через интерефейсы и DI контейнер).

Иначе: приходится писать несколько методов в одном скрипте.

* Нужно создавать абстракцию сервера, которая будет синхронизировать все события между клиентами,
вне зависимости, кто мастер-клиент. Тогда уйдет много геморая с синхронизацией.

* Не хватает слоев абстракций, из-за них код тяжело расширять т.к. измения в одном месте сильно, влияют на другие вещи.

* Нужно добавлять всегда всех имеющихся игроков.
  При входе нового игрока добавлять его. 

* Нужно серверные данные, считывать;
  изменять только через события;
  Не использовать PunMethod для передачи серверных данных, использовать только для игровых объектов и синхронизации их действий 

* Не передавать лишнюю информацию
  Не объединять серверный код с клиентским 

* Сделать две абстракции для событий: PunAbstractEvent - без разделения кода для клиента и сервера, 
  PunAbstractRequestEvent - обработка на сервере, а после обработка на клиенте 
* Вначале игры ожидать доставки всех локальных данных проблематично; поэтому нужно 
  писать код который будет применяться при вызове события, без ошибок

* Слой: Сервер и Клиент и МастерКлиент.
  Сервер можно полностью сериализовать и отправить.
  Клиенты и Мастер клиент взаимодействуют с этими данными только через события.
  События имеют свойство применяться, побочное сохранять информацию.

#### MVC
+ ControllerUi - содержат код, для взаимодействия с Model.
+ ViewUi - одно представление Model, которое может обновляться.

#### Генераторы кода

##### Пример ControllerUi

``` C#
    // Полностью регенировать все контроллеры.
    [MenuItem("Игра/Генераторы кода/Удалить контроллеры Ui")]
    public static void DeleteControllerUis()
    {
        var controllerUiGenerator = new ControllerUiGenerator(new ControllerUiData());
        controllerUiGenerator.DeleteServiceUis();
        AssetDatabase.Refresh();

        CompilationPipeline.RequestScriptCompilation();
    }

    // Запустисть генератор контроллеров.
    [MenuItem("Игра/Генераторы кода/Генерировать контроллеры Ui")]
    public static void GenerateControllerUis()
    {
        var controllerUiGenerator = new ControllerUiGenerator(new ControllerUiData());
        controllerUiGenerator.HasChanges();
        controllerUiGenerator.Generate();
    }
    
    // Стартовый метод, для всех генераторов.
    [InitializeOnLoadMethod]
    public static void StartAllGenerators()
    {
        var generators = GetGenerators();

        foreach (var generator in generators)
        {
            if (generator.HasChanges())
                generator.Generate();
        }
    }
```

