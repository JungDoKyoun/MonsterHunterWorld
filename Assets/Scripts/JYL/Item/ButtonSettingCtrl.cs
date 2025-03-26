using UnityEngine;

//버튼의 종류
public enum ButtonValue
{
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,//알파벳
    F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,//F키
    num1, num2, num3, num4, num5, num6, num7, num8, num9, num0,//숫자
    Enter, Space, Backspace, Tab, CapsLock, Shift, Ctrl, Alt, Esc, Insert, Delete, Home, End, PageUp, PageDown,//기능키
    ArrowUp, ArrowDown, ArrowLeft, ArrowRight,//방향키
    //마우스 왼쪽, 오른쪽, 보조1, 보조2
    MouseLeft, MouseRight, MouseSub1, MouseSub2,
    //마우스 휠
    MouseScrollUp, MouseScrollDown, MouseScrollclick,
    //+, -, *, ,, ., :, ;, /, \
    Plus, Minus, Multiply, Comma, Period, Colon, Semicolon, Slash, BackSlash,
    //^, @ , [, ]
    Caret, AtSymbol, OpenSquareBracket, CloseSquareBracket
}


public class ButtonSettingCtrl : MonoBehaviour
{

}
