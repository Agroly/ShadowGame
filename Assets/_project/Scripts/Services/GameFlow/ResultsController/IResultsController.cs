using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _project.Scripts.Services.GameManagement.ResultsController
{
    public interface IResultsController
    {
        public UniTask EndGame(Transform target, Quaternion rotation, CancellationToken token);
        
        public event Action<float> GameEnded;
        
    }
}