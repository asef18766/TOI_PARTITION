using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class Hole : MonoBehaviour
    {
        public UserManager userManager;
        [SerializeField] private float growthRate;
        [SerializeField] private float delay;
        [SerializeField] private Text gText;
        
        private int _groupMember;
        private int _groupId;

        public void SetGroupProperty(int idx, int cnt)
        {
            _groupMember = cnt;
            _groupId = idx;
            gText.text = (_groupId+1).ToString();
        }
        private IEnumerator Growth()
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                transform.localScale += (Vector3.one * growthRate);
            }
        }
        private void Start()
        {
            StartCoroutine(Growth());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("User"))
                return;
            var usr = other.GetComponent<User>();
            if (usr.isDead)
                return;
            userManager.SetUserGroup(usr.GetUserId(), _groupId);
            _groupMember--;
            usr.isDead = true;
            Destroy(usr.gameObject);
            if(_groupMember == 0)
                Destroy(gameObject, 0);
        }
    }
}