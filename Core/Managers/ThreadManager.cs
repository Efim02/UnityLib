namespace UnityLib.Core.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;

    using UnityLib.Architecture.Di;
    using UnityLib.Architecture.Log;
    using UnityLib.Architecture.Utils;
    using UnityLib.Core.Models.Level;

    /// <summary>
    /// Синхронизатор с потоком Unity.
    /// </summary>
    public sealed class ThreadManager : MonoBehaviour
    {
        /// <summary>
        /// Список действий.
        /// </summary>
        private List<Action> _actionList;

        /// <summary>
        /// Список действий на сцене.
        /// </summary>
        private List<Action> _sceneActionList;

        /// <summary>
        /// Дата-время изменение сцены.
        /// </summary>
        private DateTime _sceneDateTime;

        /// <summary>
        /// Initialize Manager
        /// </summary>
        private void Awake()
        {
            if (Injector.Exists<ThreadManager>())
            {
                Destroy(gameObject);
                return;
            }

            _actionList = new List<Action>();
            _sceneActionList = new List<Action>();

            Injector.RebindSingleton(this, false);
            DontDestroyOnLoad(gameObject);

            var levelChanger = Injector.Get<ILevelChanger>();
            levelChanger.LevelLoading += () => _sceneDateTime = DateTime.UtcNow;
        }

        private void Update()
        {
            if (_actionList.Count > 0)
                StartAllActions();

            if (_sceneActionList.Count > 0)
                StartAllSceneActions();
        }

        private void OnDestroy()
        {
            _actionList?.Clear();
            _sceneActionList?.Clear();
        }

        /// <summary>
        /// Синхронизировать действие.
        /// </summary>
        /// <param name="action"> Действие. </param>
        public void AddAction(Action action)
        {
            _actionList.Add(action);
        }

        /// <summary>
        /// Синхронизировать действие с задержкой.
        /// </summary>
        /// <param name="action"> Действие. </param>
        /// <param name="delay"> Задержка. </param>
        public void AddAction(float delay, Action action)
        {
            TaskUtils.TryRun(async () =>
            {
                var ms = (int)(delay * 1000);
                await Task.Delay(ms);
                _actionList.Add(action);
            });
        }

        /// <summary>
        /// Синхронизировать действие, в контексте текущей сцены.
        /// </summary>
        /// <param name="action"> Действие. </param>
        public void AddActionScene(Action action)
        {
            _sceneActionList.Add(action);
        }

        /// <summary>
        /// Синхронизировать действие с задержкой, в контексте текущей сцены.
        /// </summary>
        /// <param name="action"> Действие. </param>
        /// <param name="delay"> Задержка. </param>
        public void AddActionScene(float delay, Action action)
        {
            TaskUtils.TryRun(async () =>
            {
                var actionDateTime = _sceneDateTime;
                var ms = (int)(delay * 1000);
                await Task.Delay(ms);

                if (IsValidActionOfScene(actionDateTime))
                    _sceneActionList.Add(action);
            });
        }

        /// <summary>
        /// Это действие валидно ли для текущей сцены.
        /// </summary>
        /// <param name="dateTime"> Дата-время сцены Action. </param>
        /// <returns> True - если, валидно. </returns>
        private bool IsValidActionOfScene(DateTime dateTime)
        {
            return _sceneDateTime == dateTime;
        }

        /// <summary>
        /// Синхронизировать все добавленные действия в список.
        /// </summary>
        private void StartAllActions()
        {
            var list = _actionList.ToArray();
            _actionList.Clear();

            foreach (var action in list)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exception)
                {
                    GameLogger.Error(exception, "Менеджер потоков.");
                }
            }
        }

        /// <summary>
        /// Синхронизировать все добавленные действия в список, в контексте текущей сцены.
        /// </summary>
        private void StartAllSceneActions()
        {
            var list = _sceneActionList.ToArray();
            _sceneActionList.Clear();

            foreach (var action in list)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exception)
                {
                    GameLogger.Error(exception, "Менеджер потоков: Сцена.");
                }
            }
        }
    }
}