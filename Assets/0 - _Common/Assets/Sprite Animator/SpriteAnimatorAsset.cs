using UnityEngine;

namespace IAM
{
    [CreateAssetMenu(fileName = "Sprite Animation", menuName = "IAM/Sprite Animator Asset")]
    public class SpriteAnimatorAsset : ScriptableObject
    {
        [field: SerializeField] public Sprite[] Frames { get; private set; }
        [field: SerializeField] public float FrameRate { get; private set; } = 0.25f;
        [field: SerializeField] public bool IsLoop { get; private set; } = true;
        [field: SerializeField] public bool RandomOrder { get; private set; } = false;
        [field: SerializeField] public bool AvoidRepeatingLastSprite { get; private set; } = false;
        [field: SerializeField] public float PlaybackDistortion { get; private set; } = 0;
    }
}
