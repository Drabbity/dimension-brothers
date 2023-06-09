using DimensionBrothers.Audio;
using DimensionBrothers.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace DimensionBrothers.Dimension
{
    public class DimensionManager : MonoBehaviour
    {
        [SerializeField] private Dimension[] _dimensions = new Dimension[2];
        [SerializeField] private int _startingDimensionIndex = 0;

        private int? _activeDimensionIndex = null;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = AudioManager.Instance;
            PopulateObjectArray();
            ChangeDimension(_startingDimensionIndex);
        }

        public void Update()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                ChangeDimension(0);
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                ChangeDimension(1);
        }
        
        public void ChangeDimension(int index)
        {
            if (_activeDimensionIndex == null)
            {
                for (int i = 0; i < _dimensions.Length; i++)
                    ActivateDimension(i, false);

                ActivateDimension(index, true);
                _activeDimensionIndex = index;
            }
            else
            {
                _audioManager.PlaySound("FLIP");
                ActivateDimension((int)_activeDimensionIndex, false);
                ActivateDimension(index, true);
                _activeDimensionIndex = index;
            }
        }

        private void ActivateDimension(int index, bool isActive)
        {
            _dimensions[index].PlayerController.IsActive = isActive;
            _dimensions[index].TilemapRenderer.enabled = isActive;

            foreach(var dimensionObject in _dimensions[index].Objects)
            {
                dimensionObject.enabled = isActive;
            }
        }

        private void PopulateObjectArray()
        {
            for(int i = 0; i < _dimensions.Length; i++)
            {
                _dimensions[i].Objects = _dimensions[i].TilemapRenderer.transform.GetComponentsInChildren<SpriteRenderer>(true);
            }
        }

        [Serializable]
        struct Dimension
        {
            public PlayerController PlayerController;
            public TilemapRenderer TilemapRenderer;
            [HideInInspector] public SpriteRenderer[] Objects;
        }
    }
}