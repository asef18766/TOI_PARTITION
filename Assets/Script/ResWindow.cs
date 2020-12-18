using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class ResWindow : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Text teammate;
        [SerializeField] private Button btn;

        private Dictionary<int, List<string>> _team;
        
        public void SetDicData(Dictionary<int, List<string>> t)
        {
            _team = t;
        }
        
        private int _counter;
        private static readonly int Pop = Animator.StringToHash("Pop");

        public void DisplayNext()
        {
            if (!_team.ContainsKey(_counter))
            {
                Destroy(gameObject);
                return;
            }
            title.text = $"第 {_counter+1} 組";
            teammate.text = _team[_counter].Aggregate("", (current, s) => current + $"{s}\n");
            _counter++;
            GetComponent<Animator>().SetTrigger(Pop);
        }
    }
}