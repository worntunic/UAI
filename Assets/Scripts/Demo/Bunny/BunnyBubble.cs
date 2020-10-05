using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAI.Demo
{
    [System.Serializable]
    public struct BubbleImage
    {
        public string key;
        public GameObject image;
    }
    public class BunnyBubble : MonoBehaviour
    {
        public List<BubbleImage> images = new List<BubbleImage>();
        private int currentImage;

        private void Start()
        {
            if (images.Count != 0)
            {
                currentImage = 0;
                foreach (BubbleImage bubbleImage in images)
                {
                    bubbleImage.image.SetActive(false);
                } 
            }
        }
        public void SetImage(string key)
        {
            int newImage = currentImage;
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].key == key)
                {
                    newImage = i;
                    break;
                }
            }
            images[currentImage].image.SetActive(false);
            currentImage = newImage;
            images[currentImage].image.SetActive(true);
        }

        void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
        }
    }
}

