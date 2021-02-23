using UnityEngine;

public class PlayerSlowmoManager : MonoBehaviour
{
    #region Events



    #endregion

    #region Inspector

    [SerializeField, Range(0f, 1f), Tooltip("How strong is the slowmo effect")]
    private float slowmo;

    [SerializeField, Range(0f, 10f), Tooltip("How long does the slowmo last")]
    private float slowmoDurr;

    [SerializeField, Range(0f, 10f), Tooltip("How fast does the slowmo recover")]
    private float slowmoRecover;

    [SerializeField, Range(0f, 1f), Tooltip("How much must the counter be filled until the player can use the slowmo again after it reached zero")]
    private float minSlowmo;

    #endregion

    /// <summary>
    /// Is the slowmotion enabled?
    /// </summary>
    public bool isSlowmo 
    { 
        get => Time.timeScale == slowmo; 
        private set => Time.timeScale = value ? slowmo : 1;
    }

    private bool isDisabled;        // If true the counter reached zero and needs to count up again
    private float counter;

    #region Debug

    [Space(20f)]
    public bool debug;

    private void OnGUI()
    {
        if (!debug) return;

        GUILayout.Box("isSlowmo = " + isSlowmo.ToString());
        GUILayout.Box("SlowmoCounter = " + counter.ToString());
    }

    #endregion

    private void Update()
    {
        if (isSlowmo)
        {
            // Count down
            counter = Mathf.Clamp(counter - Time.unscaledDeltaTime, 0, slowmoDurr);

            if (counter <= 0)
            {
                // Disable slow mo because the time ran out
                isSlowmo = false;
                isDisabled = true;
            }
        }
        else
        {
            // Recover slowmo
            counter = Mathf.Clamp(counter + Time.deltaTime * slowmoRecover, 0, slowmoDurr);
            
            if (counter >= slowmoDurr * minSlowmo)
            {
                // Enable Slowmo again
                isDisabled = false;
            }
        }
    }

    public void ToggleSlowmo()
    {
        if (isDisabled) return;

        isSlowmo = !isSlowmo;
    }

    private void Awake() => counter = slowmoDurr;
}
