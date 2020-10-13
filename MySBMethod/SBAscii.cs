using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;

namespace SBMethod
{
    /// <summary>
    /// SmallBasic用アスキー制御コード
    /// </summary>
    [SmallBasicType]
    public static class SBAscii
    {
        /// <summary>
        /// ヌル文字　Null
        /// 文字列の終端を表すのに用います
        /// </summary>
        public static Primitive NUL { get; } = (char)0x00;
        /// <summary>
        /// ヘッディング開始(ヘッダ開始)　Start of Heading
        /// 通信伝文中のヘッダ開始を表します
        /// </summary>
        public static Primitive SOH { get; } = (char)0x01;
        /// <summary>
        /// テキスト開始　Start of Text
        /// 通信伝文中のテキスト部分の開始を表します
        /// </summary>
        public static Primitive STX { get; } = (char)0x02;
        /// <summary>
        /// テキスト終了　End of Text
        /// 通信伝文中のテキスト部分の終了を表します
        /// </summary>
        public static Primitive ETX { get; } = (char)0x03;
        /// <summary>
        /// 伝送終了(転送終了)　End of Transmission
        /// データ送信側がデータ送信終了時にデータ受信先にEOTを送ります
        /// </summary>
        public static Primitive EOT { get; } = (char)0x04;
        /// <summary>
        /// 問い合わせ(照会)　Enquiry
        /// データ送信側がデータ送信しようというときに、
        /// データ受信側にデータに先立ってENQを送ります
        /// データ受信先は、データ受信できる状態であればデータ送信側にACKを送り、
        /// データ受信できない状態であればNAKを送ります
        /// データ送信側はACKを受信した場合にデータを送り、
        /// NAKを受信した場合はデータ送信を断念したり時間を置いて
        /// 再度ENQ送信するなどの処理を行います
        /// </summary>
        public static Primitive ENQ { get; } = (char)0x05;
        /// <summary>
        /// 肯定応答(受信確認)　Acknowledge
        /// 受信したデータにCRCなどの異常がない場合や、
        /// ENQを受信後にデータ受信ができる状態であれば、送信側にACKを送ります
        /// </summary>
        public static Primitive ACK { get; } = (char)0x06;
        /// <summary>
        /// ベル(警告)　Bell
        /// ビープ音を鳴らします
        /// </summary>
        public static Primitive BEL { get; } = (char)0x07;
        /// <summary>
        /// 後退　Backspace
        /// カーソルを手前（左）に移動させてそこの文字を削除します
        /// </summary>
        public static Primitive BS { get; } = (char)0x08;
        /// <summary>
        /// 水平タブ　Horizontal Tabulation
        /// 水平方向のタブ
        /// </summary>
        public static Primitive HT { get; } = (char)0x09;
        /// <summary>
        /// 改行　Line Feed
        /// カーソルを桁（水平方向）はそのままで1行下へ移動させます
        /// </summary>
        public static Primitive LF { get; } = (char)0x0A;
        /// <summary>
        /// 垂直タブ　Vertical Tabulation
        /// 垂直方向のタブ
        /// </summary>
        public static Primitive VT { get; } = (char)0x0B;
        /// <summary>
        /// 書式送り(改頁)　Form Feed
        /// コードの論理的区分の分け目として使用されます
        /// </summary>
        public static Primitive FF { get; } = (char)0x0C;
        /// <summary>
        /// 復帰　Carriage Return
        /// カーソルを同じ行の先頭の桁（左端）へ移動します
        /// </summary>
        public static Primitive CR { get; } = (char)0x0D;
        /// <summary>
        /// シフトアウト　Shift Out
        /// 別の文字コードセットに遷移します
        /// </summary>
        public static Primitive SO { get; } = (char)0x0E;
        /// <summary>
        /// シフトイン　Shift In
        /// 通常の文字コードセットに遷移します
        /// </summary>
        public static Primitive SI { get; } = (char)0x0F;
        /// <summary>
        /// 伝送制御拡張(データリンクエスケー プ)　Data Link Escape
        /// バイナリ通信（データそのものに制御文字を含むような通信）で
        /// あることを表すために使います
        /// </summary>
        public static Primitive DLE { get; } = (char)0x10;
        /// <summary>
        /// 装置制御１(XON)　Device Control 1
        /// 装置制御のために予約されています
        /// </summary>
        public static Primitive DC1 { get; } = (char)0x11;
        /// <summary>
        /// 装置制御２　Device Control 2
        /// 装置制御のために予約されています
        /// </summary>
        public static Primitive DC2 { get; } = (char)0x12;
        /// <summary>
        /// 装置制御３(XOFF)　Device Control 3
        /// 装置制御のために予約されています
        /// </summary>
        public static Primitive DC3 { get; } = (char)0x13;
        /// <summary>
        /// 装置制御４　Device Control 4
        /// 装置制御のために予約されています
        /// </summary>
        public static Primitive DC4 { get; } = (char)0x14;
        /// <summary>
        /// 否定応答(受信失敗)　Negative Acknowledge
        /// 受信したデータにCRCなどの異常があった場合や、
        /// ENQを受信後にデータ受信ができる状態でないなら
        /// 送信側にNAKを送ります
        /// </summary>
        public static Primitive NAK { get; } = (char)0x15;
        /// <summary>
        /// 同期信号　Synchronous Idle
        /// キャラクタ同期方式の通信で、同期を取るために使用します
        /// </summary>
        public static Primitive SYN { get; } = (char)0x16;
        /// <summary>
        /// 転送ブロック終結(終了)　End of Transmission Block
        /// </summary>
        public static Primitive ETB { get; } = (char)0x17;
        /// <summary>
        /// 取消(キャンセル)　Cancel
        /// 先行するデータにエラーがある、
        /// または、無視してほしいことを表します
        /// </summary>
        public static Primitive CAN { get; } = (char)0x18;
        /// <summary>
        /// 媒体終端(メディア終了)　End of Medium
        /// 受信データを記録する媒体が、
        /// 記録できる範囲の末端まで到達したことを表します
        /// </summary>
        public static Primitive EM { get; } = (char)0x19;
        /// <summary>
        /// 置換　Substitute Character
        /// 本来は、伝送制御文字として、不明瞭な、または、
        /// 無効な文字を受信したことを表します。
        /// しかし、下位レイヤで誤り検出訂正が行われるため、
        /// この用途で用いる必要はほぼなく、他の用途で用いられます
        /// テキストファイルのファイル終端（EOF)を表すのによく使われます
        /// </summary>
        public static Primitive SUB { get; } = (char)0x1A;
        /// <summary>
        /// エスケープ　Escape
        /// キーボードのEscキーを押すとこの文字がシステムに送られます
        /// </summary>
        public static Primitive ESC { get; } = (char)0x1B;
        /// <summary>
        /// ファイル分離標識(フォーム区切り)　File Separator
        /// データ構造のフィールドを記録する区切り文字として使われます
        /// </summary>
        public static Primitive FS { get; } = (char)0x1C;
        /// <summary>
        /// グループ分離標識(グループ区切り)　Group Separator
        /// データ構造のフィールドを記録する区切り文字として使われます
        /// </summary>
        public static Primitive GS { get; } = (char)0x1D;
        /// <summary>
        /// レコード分離標識(レコード区切り)　Record Separator
        /// データ構造のフィールドを記録する区切り文字として使われます
        /// </summary>
        public static Primitive RS { get; } = (char)0x1E;
        /// <summary>
        /// ユニット分離標識(ユニット区切り)　Unit Separator
        /// データ構造のフィールドを記録する区切り文字として使われます
        /// </summary>
        public static Primitive US { get; } = (char)0x1F;
        /// <summary>
        /// 抹消(削除)　Delete
        /// カーソルのすぐ右の文字を削除するのに使われます
        /// </summary>
        public static Primitive DEL { get; } = (char)0x7F;
        /// <summary>
        /// 復帰(文字列)
        /// </summary>
        public static Primitive StrCr { get; } = "\n";
        /// <summary>
        /// 改行(文字列)
        /// </summary>
        public static Primitive StrLf { get; } = "\r";
        /// <summary>
        /// 改行＆復帰(文字列)
        /// </summary>
        public static Primitive StrCrLf { get; } = "\n\r";
        /// <summary>
        /// タブ(文字列)
        /// </summary>
        public static Primitive StrTAB { get; } = "\t";
    }
}
