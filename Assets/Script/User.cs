using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script
{
    public class User : MonoBehaviour
    {
        [SerializeField] private Text username;
        [SerializeField] private float speed;

        [SerializeField] private Vector2 startPos;
        [SerializeField] private Vector2 delta;
        [SerializeField] private Vector2 rnc;
        public bool isDead = false;
        private int _userId;
        private Rigidbody2D _rb;
        private RectTransform _rectTransform;

        public int GetUserId()
        {
            return _userId;
        }
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rectTransform = GetComponent<RectTransform>();
        }
        public void SetName(string userName, int usrId)
        {
            username.text = userName;
            _userId = usrId;
        }

        public void SetBounce()
        {
            var ro = Random.Range(0, 360);
            print("push!!");
            _rb.AddForce(new Vector2(Convert.ToSingle(Math.Cos(ro)), Convert.ToSingle(Math.Sin(ro))) * speed);
        }

        public void SetPos(int order)
        {
            var dx = order % Convert.ToInt32(rnc.x);
            var dy = order / Convert.ToInt32(rnc.y);
            _rectTransform.localPosition = startPos + new Vector2(delta.x * dx, delta.y * dy);
        }
    }
}
