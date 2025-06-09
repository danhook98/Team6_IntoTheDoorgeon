using System.Collections;
using UnityEngine;

namespace SkyMapper
{
    [RequireComponent(typeof(Camera))]
    public class AspectRatioLock : MonoBehaviour
    {
        [SerializeField] private Vector2 _targetAspectRatio = new(16, 9);
        
        private Camera _camera;
        
        private float _desiredAspectRatio;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _desiredAspectRatio = _targetAspectRatio.x / _targetAspectRatio.y;
        }

        private void Start()
        {
            UpdateCameraRect();
            StartCoroutine(CheckAspectChanged());
        }

        /// <summary>
        /// Continuously checks if the screen resolution has changed, and then updates the camera rect.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckAspectChanged()
        {
            // Store the screen's width and height.
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            
            // Cache the delay object for the while loop.
            WaitForSeconds delay = new WaitForSeconds(0.25f);

            while (true)
            {
                // Update the camera's rect if the screen resolution has changed.
                if (screenWidth != Screen.width || screenHeight != Screen.height)
                {
                    UpdateCameraRect();
                    screenWidth = Screen.width;
                    screenHeight = Screen.height;
                }

                yield return delay;
            }
        }

        /// <summary>
        /// Modifies the rect of the attached camera to enforce the defined 'desired' aspect ratio.
        /// </summary>
        private void UpdateCameraRect()
        {
            // Get the current camera aspect ratio. 
            float aspectRatio = (float) Screen.width / (float) Screen.height;
            
            // Get the camera's rect. 
            Rect cameraRect = _camera.rect;
            
            float scaleHeight = aspectRatio / _desiredAspectRatio;

            switch (scaleHeight)
            {
                // If scale height is less than 1, then the screen is too tall compared to our desired aspect ratio, so we
                // need to add a letterbox (black bars at the top and bottom). 
                case < 1.0f:
                {
                    // Set the width of the camera rect to the full width of the screen.
                    cameraRect.width = 1.0f;

                    // Set the height of the camera rect to the calculated scale height, this ensures that the height
                    // matches our desired 16:9 aspect ratio. 
                    cameraRect.height = scaleHeight;

                    cameraRect.x = 0f;

                    // Reposition the rect to ensure it's centred on the Y axis.
                    cameraRect.y = (1.0f - scaleHeight) / 2.0f;
                    
                    break;
                }
                // If the scale height is larger than 1, then the screen is too wide compared to our desired aspect ratio, so
                // we need to add a pillar box (black bars at the sides).
                case > 1.0f:
                {
                    // Calculate the new rect width. 
                    float scaleWidth = 1.0f / scaleHeight;
                
                    // Set the width of the camera rect to the calculated scale width, this ensures that the width matches
                    // our desired 16:9 aspect ratio. 
                    cameraRect.width = scaleWidth;
                
                    // Set the height of the camera rect to the full height of the screen.
                    cameraRect.height = 1f;
                
                    // Reposition the rect to ensure it's centred on the X axis.
                    cameraRect.x = (1.0f - scaleWidth) / 2.0f;
                
                    cameraRect.y = 0f;
                    
                    break;
                }
            }
            
            // Update the camera's rect.
            _camera.rect = cameraRect;
        }
    }
}
