using UnityEngine;

public class FPS : MonoBehaviour
{
    private float _fps;
    private GUIStyle _style = new GUIStyle();
    private Rect _rect = new Rect();
    private float lastTime = 0f;
    private float delay = .2f;
    private string _fpsString = "";
    void Awake()
    {
        int width = Screen.width;
        int height = Screen.height;
        _rect = new Rect(0, 0, width, height * 0.3f);

        _style.alignment = TextAnchor.UpperCenter;
        _style.fontSize = height * 6 / 100;
        _style.normal.textColor = Color.black;
    }

    private void OnGUI()
    {
        GUI.Label(_rect, _fpsString, _style);

        if (Time.time < lastTime ) return;
        lastTime = Time.time + delay;

        _fps =  1f / Time.unscaledDeltaTime;
        _fps = (int)_fps;
        _fpsString = _fps.ToString();
    }
}
