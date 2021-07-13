using UnityEngine;

namespace Arena
{
    public class GridEntity : MonoBehaviour
    {
        private void Start()
        {
            GameArena.Instance.Register(this);
        }

        public void Move(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}