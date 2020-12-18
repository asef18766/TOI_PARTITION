using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Script
{
    public class UserManager : MonoBehaviour
    {
        private InputStruct _inputStruct;
        private List<User> _userCollection = new List<User>();
        
        [SerializeField] private GameObject userPref;
        [SerializeField] private GameObject holePref;
        [SerializeField] private Transform canvas;
        [SerializeField] private string fileLoc;
        [SerializeField] private float delayTime;
        [SerializeField] private float holeRad;
        [SerializeField] private ResWindow resWindow;
        [SerializeField] private GameObject loadingWindow;
        
        private List<Hole> _holes = new List<Hole>();
        private IEnumerator Start()
        {
            var request = UnityWebRequest.Get(fileLoc);
            loadingWindow.SetActive(true);
            yield return request.SendWebRequest();
            var json = request.downloadHandler.text;
            Debug.Log(json);
            _inputStruct = JsonConvert.DeserializeObject<InputStruct>(json);
            loadingWindow.SetActive(false);
            for (var i = 0 ; i != _inputStruct.participants.Length ; ++i)
            {
                var usr = _inputStruct.participants[i];
                var nUser = Instantiate(userPref, canvas).GetComponent<User>();
                nUser.SetName(usr, i);
                nUser.SetPos(i);
                _userCollection.Add(nUser);
            }

            for (var i = 0; i != _inputStruct.groupRule.Length; ++i)
            {
                var go = Instantiate(holePref, canvas);
                go.SetActive(false);
                var hole = go.GetComponent<Hole>();
                hole.SetGroupProperty(i, _inputStruct.groupRule[i]);
                hole.userManager = this;

                var theta = i * (Math.PI*2 / _inputStruct.groupRule.Length);
                hole.GetComponent<RectTransform>().localPosition = new Vector3(
                        holeRad * Convert.ToSingle(Math.Cos(theta)),
                        holeRad * Convert.ToSingle(Math.Sin(theta)),
                        0
                        );
                _holes.Add(hole);
            }
        }

        private Dictionary<int, List<int>> _group = new Dictionary<int, List<int>>();

        private bool _checkGroup()
        {
            for (var  i = 0 ; i != _inputStruct.groupRule.Length ; ++i)
            {
                if (!_group.ContainsKey(i))
                    return false;
                if (_inputStruct.groupRule[i] != _group[i].Count)
                    return false;
            }
            return true;
        }
        public void SetUserGroup(int userId, int groupId)
        {
            print($"user {userId} add into {groupId}");
            if(!_group.ContainsKey(groupId))
                _group.Add(groupId, new List<int>());
            if (_group[groupId].Count == _inputStruct.groupRule[groupId])
            {
                print("Error!!");
                throw new ArgumentException();
            }

            _group[groupId].Add(userId);

            if (!_checkGroup()) return;
            print("done~~");
            var groupF = new Dictionary<int, List<string>>();
            foreach (var kvp in _group)
            {
                groupF.Add(kvp.Key, new List<string>());
                foreach (var uid in kvp.Value)
                {
                    groupF[kvp.Key].Add(_inputStruct.participants[uid]);
                }
            }
            resWindow.gameObject.SetActive(true);
            resWindow.SetDicData(groupF);
            resWindow.DisplayNext();
        }
        public void Bounce()
        {
            foreach (var usr in _userCollection)
            {
                usr.SetBounce();
            }

            foreach (var h in _holes)
            {
                h.gameObject.SetActive(true);
            }
        }
        
    }
}