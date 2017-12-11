
' モンテカルロ法

Imports System.Math
Public Class Monte_Carlo_Calc_PI
	' 範囲内dot
	Private Structure Circle
		' 範囲内
		Dim in_dot As Integer
		' 全体
		Dim all_dot As Integer
	End Structure
	Private cc As Circle

	' dotのサイズ
	Private g_cr As Integer = 2

	' 最大最長
	Private Structure msls
		Dim min As Double
		Dim max As Double
	End Structure

	' 範囲
	Private def_x As msls
	Private def_y As msls

	' 半径
	Private def_r As Double

	' コンストラクタ
	Public Sub New()
		' モードレスでForm起動
		Monte_Carlo_Circle_Form.Show()
	End Sub

	Public Sub reset()
		cc.in_dot = 0
		cc.all_dot = 0

		def_x.min = 0
		def_x.max = 1
		def_y.min = 0
		def_y.max = 1

		def_r = 1

		' モードレスでForm起動
		Monte_Carlo_Circle_Form.Show()
		Monte_Carlo_Circle_Form.graph_init(def_x.min, def_x.max, def_y.min, def_y.max)

		' 範囲を表示
		'Dim i As Double
		'For i = def_x.min To def_x.max Step 0.001
		'	Monte_Carlo_Circle_Form.graph_dot(i, func_circle(i), g_cr, Pens.Blue)
		'Next i

	End Sub

	Public Sub update(form1_n As Integer)
		calc_circle_MC()

	End Sub

	Private Sub calc_circle_MC()

		'シード値として Environment.TickCount が使用される
		Dim rand As New System.Random()
		Dim rand_x As Double = rand.NextDouble() ' * (def_x.max - def_x.min) + def_x.min
		Dim rand_y As Double = rand.NextDouble() ' * (def_y.max - def_y.min) + def_y.min

		If rand_y <= func_circle(rand_x) Then
			cc.in_dot = cc.in_dot + 1
			Monte_Carlo_Circle_Form.graph_dot(rand_x, rand_y, g_cr, Pens.Green)
		Else
			Monte_Carlo_Circle_Form.graph_dot(rand_x, rand_y, g_cr, Pens.Red)
		End If


		cc.all_dot = cc.all_dot + 1

	End Sub

	Private Function func_circle(x As Double)
		Dim y As Double
		y = Sqrt(def_r ^ 2 - x ^ 2)
		Return y
	End Function

	Public Function get_calc_circle_MC()
		Return cc.in_dot / cc.all_dot * 4.0
	End Function


End Class
