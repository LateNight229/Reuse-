using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingState : MonoBehaviour, GameStateMachine
{
    [Header("serialize")]
    public GameObject UIObject;
    Main MainScript;
    public static LoadingState instance;
    void Awake()
    {
        Main.LoadState(this, 0, true);
        instance = this;
        UIObject = GameObject.Find("Loading");
        GameObject gameStateObj = GameObject.Find("GameState");
        gameState = gameStateObj.GetComponent<GameState>();
        _gameUIObj = gameStateObj;
        _loginUI = UIObject.GetComponent<LoginUI>();
        _settingUI = GameObject.Find("All").transform.Find("SettingUI").GetComponent<SettingUI>();
        

    }
    public IEnumerator Load(Main _MainScript)
    {
        MainScript = _MainScript;
        yield return null;
    }
    public void UpdateState()
    {

    }
    public void LateUpdateState()
    {

    }
    public void FixedUpdateState()
    {

    }
    public void StartInput(Interaction.Data _Data)
    {

    }
    public void UpdateInputMove(Interaction.Data _Data)
    {

    }
    public void UpdateInputStationary(Interaction.Data _Data)
    {

    }
    public void EndInput(Interaction.Data _Data)
    {

    }
    public void DoChangeState(System.Type _Type, ChangeStateEffect.EffectType _EffectType, float _TimeChange)
    {
        MainScript.StartChangeState(MainScript.GetState(_Type), _EffectType, _TimeChange);
    }
    public void ShutDown()
    {

    }
    public void Enable()
    {
        gameObject.SetActive(true);
        if (UIObject != null)
            UIObject.SetActive(true);
        print("Enable");
        SoundManager.Instance.PlayMusic(Config.Music.MUSIC_1);
        StartCoroutine(Init());
    }
    public void Disable()
    {
        if (UIObject != null)
            UIObject.SetActive(false);
    }
    public void StartChangeState(GameStateMachine _NextState, GameStateMachine _LastState)
    {
        if (_NextState.GetType() == GetType())
        {
            Enable();
        }
    }
    public void UpdateChangeState(GameStateMachine _NextState, GameStateMachine _LastState, float _Time)
    {

    }
    public void SetChangeState(GameStateMachine _NextState, GameStateMachine _LastState)
    {

    }
    public void EndChangeState(GameStateMachine _NextState, GameStateMachine _LastState)
    {
        if (_NextState.GetType() != GetType())
        {
            Disable();
        }
    }
    public void UpdateUIPosition(Vector3 localPosition)
    {
        UIObject.transform.localPosition = localPosition;
    }
    public void UpdateUISortingOder()
    {
        //UIObject.transform.SetAsLastSibling();
    }

    void Finish()
    {
        DoChangeState(typeof(GameState), ChangeStateEffect.EffectType.None, 0);
    }

    LoginUI _loginUI;
    GameObject _gameUIObj;
    GameState gameState;
    SettingUI _settingUI;

    public IEnumerator Init()
    {
        _loginUI.Init(CallbackPlayNow);
        ButtonSettingClick.instance.Init(CallbackClickSetting);
        yield return Login();

    }

    public IEnumerator LoadData()
    {   
        //yield return API.LoadMasterData();
        //yield return API.LoadPlayerData();
        yield return StartCoroutine(AnimateFillToFull(1f));
        SlashScreenControl.instance.Hide();
        StartCoroutine(gameState.Init());
        _gameUIObj.SetActive(true);
        Finish();
    }

    public IEnumerator Login()
    {
        _gameUIObj.SetActive(false);
        _loginUI.Show();
        _settingUI.Init();
        yield return null;
    }

    public void CallbackPlayNow()
    {   
        StartCoroutine(LoadData());
    }

    public void CallbackClickSetting()
    {
        _settingUI.Show();
    }

    /// <summary>
    /// Animate thanh loading từ giá trị hiện tại lên 100% trong duration giây.
    /// </summary>
    private IEnumerator AnimateFillToFull(float duration)
    {
        // Nếu SlashScreenControl có hàm lấy tỉ lệ hiện tại, dùng nó; 
        // nếu không, giả sử bạn khởi đầu từ 0.
        float startFill = 0f;
        // Ví dụ: startFill = SlashScreenControl.instance.GetFillAmount();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;                     // 0 → 1
            float current = Mathf.Lerp(startFill, 1f, t);     // fill interpolated
            SlashScreenControl.instance.UpdateFillBar(current);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Đảm bảo kết quả là 100%
        SlashScreenControl.instance.UpdateFillBar(1f);
    }

}
