using UnityEngine;

[CreateAssetMenu(fileName = "CustomerVisualProfile", menuName = "KofiSim/CustomerVisualProfile")]
public class CustomerVisualProfile : ScriptableObject
{
    public Sprite idleSprite; // Sprite para cuando el cliente está quito
    public Sprite walkRightSprite; // Sprite para cuando el cliente está caminando hacia la derecha
    public Sprite walkLeftSprite; // Sprite para cuando el cliente está caminando hacia la izquierda
}
