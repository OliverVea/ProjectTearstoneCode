using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRpg.Core
{
    public class TaskManager : MonoBehaviour
    {
        [Serializable]
        private class Task
        {
            public Action Action;
            public float secondsBetweenExecutions;
            public float secondsSinceLastExecution;
        }

        private static List<Task> _tasks;
        [SerializeField] private List<Task> taskMirror;

        private void Update()
        {
            foreach (var task in _tasks) ExecuteTask(task);
            taskMirror = _tasks.ToList();
        }

        private void OnEnable()
        {
            _tasks = new List<Task>();
        }

        private void ExecuteTask(Task task)
        {
            task.secondsSinceLastExecution += Time.deltaTime;
            if (task.secondsBetweenExecutions > task.secondsSinceLastExecution) return;

            task.Action();
            
            task.secondsSinceLastExecution = 0;
        }

        public static void AddTask(Action action, float secondsBetweenExecutions)
        {
            _tasks.Add(new Task
            {
                Action = action,
                secondsBetweenExecutions = secondsBetweenExecutions
            });
        }
    }
}