'
' x=1025でオーバーフローが発生する
'ガウス = ルジャンドルのアルゴリズム
'


Public Class Form1
	'====================================================
	' 構造体
	'====================================================
	'最大最小構造体
	'most less
	Private Structure msls
		Dim min As Double
		Dim max As Double

	End Structure

	'====================================================
	' グローバル変数定義
	'====================================================
	' x軸y軸
	Dim def_x As msls
	Dim def_y As msls

	' x軸の最大
	Private n_MAX As Double = 2000

	' 束縛変数n
	Private n As Integer = 0

	' dot の直径
	Private g_cr As Integer

	' 処理時間
	Private g_sw As New System.Diagnostics.Stopwatch

	' グラフ関係のクラス
	'Private g_graph As Graphics
	Private show_dot_graph As New Show_Dot_Graph

	' Simga_Calc_PIクラス
	Private sigma_calc_PI As New Sigma_Calc_PI

	' Integral_Calc_PIクラス
	Private integral_calc_PI As New Integral_Calc_PI

	' Web_Data_Array_PIクラス
	'Private web_data_arry_PI As New Web_Data_Array_PI

	'Monte_Carlo_Calc_PI.vb クラス
	Private monte_carlo_calc_PI As New Monte_Carlo_Calc_PI


	'====================================================
	' イベント処理
	'====================================================
	'
	' 起動時処理
	'
	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		'Visual BasicのSystem.Mathが定義している，PIを出力
		TextBox1002.Text = Math.PI

		TextBox1.Text = n_MAX
		TextBox2.Text = -1
		TextBox3.Text = n_MAX + 10
		TextBox4.Text = 0
		TextBox5.Text = 3.5

		TextBox6.Text = 1

		TextBox7.Text = 100
		TextBox8.Text = 1


		' Form2をモードレスで表示する
		' モードレス...Form1と同時に操作できる <=(対比)=> モーダル
		show_dot_graph.Show()

	End Sub

	'
	' INITボタン
	'
	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		' nの最大値
		n_MAX = TextBox1.Text

		' グラフのx軸範囲
		def_x.min = TextBox2.Text
		def_x.max = TextBox3.Text

		' グラフのy軸範囲
		def_y.min = TextBox4.Text
		def_y.max = TextBox5.Text

		' StopWatch リセット
		g_sw.Reset()

		' dotの大きさ
		g_cr = TextBox6.Text

		' 座標軸
		show_dot_graph.graph_init(def_x.min, def_x.max, def_y.min, def_y.max, TextBox7.Text, TextBox8.Text)

		sigma_calc_PI.reset()
		integral_calc_PI.reset(n_MAX)
		'web_data_arry_PI.reset()
		monte_carlo_calc_PI.reset()

		' nの初期化
		n = 0

		' ProgressBar
		ProgressBar1.Minimum = n
		ProgressBar1.Maximum = n_MAX + 1

	End Sub

	'
	' START/STOPボタン
	'
	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

		If n >= n_MAX Then
			' グラフ範囲外なら終了，始まらない
			Timer1.Enabled = False
		ElseIf Timer1.Enabled = True Then
			' 動いてたら止める
			Timer1.Enabled = False
			g_sw.Stop()
		ElseIf Timer1.Enabled = False Then
			' 止まってたら動く
			Timer1.Enabled = True
			g_sw.Start()
		End If

	End Sub


	'
	' タイマー
	'
	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
		' グラフ範囲外なら終了
		If n >= n_MAX Then
			Timer1.Enabled = False
		End If

		' 現在のnの表示
		TextBox1001.Text = n

		' 現在の所要時間の表示
		TextBox1003.Text = g_sw.Elapsed.ToString

		' 各種PIの出力と描画
		show_pi_value()

		' 計算を進める
		sigma_calc_PI.update(n)
		integral_calc_PI.update(n)
		monte_carlo_calc_PI.update(n)

		' nを進める
		n = n + 1

		' ProgressBarを進める
		ProgressBar1.Value = n
	End Sub

	'====================================================
	' 初期化
	'====================================================
	'
	' 特になし
	'


	'====================================================
	' 自作関数
	'====================================================
	'
	' 各PIのグラフとTextBoxへの出力
	'
	Private Sub show_pi_value()
		show_pi_data(TextBox101, TextBox201, TextBox301, Color.Blue, Pens.Blue, sigma_calc_PI.get_Leibniz())
		show_pi_data(TextBox102, TextBox202, TextBox302, Color.Green, Pens.Green, sigma_calc_PI.get_zeta_4())
		show_pi_data(TextBox103, TextBox203, TextBox303, Color.Orange, Pens.Orange, sigma_calc_PI.get_Gauss_Legendre())
		show_pi_data(TextBox104, TextBox204, TextBox304, Color.YellowGreen, Pens.YellowGreen, sigma_calc_PI.get_Gauss_Legendre())

		show_pi_data(TextBox105, TextBox205, TextBox305, Color.Brown, Pens.Brown, integral_calc_PI.get_Gaussian())
		show_pi_data(TextBox106, TextBox206, TextBox306, Color.Purple, Pens.Purple, integral_calc_PI.get_seki_1())
		show_pi_data(TextBox107, TextBox207, TextBox307, Color.Aqua, Pens.Aqua, integral_calc_PI.get_seki_2())

		show_pi_data(TextBox108, TextBox208, TextBox308, Color.MediumSpringGreen, Pens.MediumSpringGreen, monte_carlo_calc_PI.get_calc_circle_MC())
	End Sub

	'
	' グラフへのdotとTextBoxへの代入を行う
	'
	Private Sub show_pi_data(text_pi As TextBox, text_sub As TextBox, text_status As TextBox, t_color As Color, p_Color As Pen, pi As Double)

		'点の直径
		Dim cr As Integer = g_cr



		If Not Double.IsNaN(pi) Then
			' piが非NaNのとき
			text_status.ForeColor = Color.Black
			text_status.Text = "演算中"

			text_pi.Text = pi
			text_sub.Text = pi - Math.PI

			text_pi.ForeColor = t_color

			'form2でグラフ表示
			show_dot_graph.graph_dot(n, pi, cr, p_Color)
		Else
			' piがNaNのとき
			text_status.ForeColor = Color.Red
			text_status.Text = "演算エラー"
		End If


	End Sub

End Class


'========================
'   EOF Form1.vb
'========================
