

' ガウス積分

Imports System.Math
Public Class Integral_Calc_PI
	' 現在の束縛変数n
	Private n As Integer
	Private n_max As Integer

	' 無限とは？
	Private INFINITY As Double = 1.0 * 10 ^ 3


	Private Structure Integral
		Dim a As Double ' 積分範囲 a->b
		Dim b As Double
		Dim t As Double ' 束縛変数
		Dim tmp As Double ' t時点での値
	End Structure
	' ガウス積分
	Private gi As Integral

	' wikipediaより
	Private seki_1 As Integral
	Private seki_2 As Integral

	' 初期化関数
	Public Sub reset(form1_max_n As Integer)
		n_max = form1_max_n


		' ガウス積分
		gi.a = -INFINITY
		gi.b = INFINITY
		gi.t = get_t(gi.a, gi.b)
		gi.tmp = 0

		' 積分1
		seki_1.a = -1
		seki_1.b = 1
		seki_1.t = get_t(seki_1.a, seki_1.b)
		seki_1.tmp = 0

		' 積分2
		seki_2.a = 0
		seki_2.b = 1
		seki_2.t = get_t(seki_2.a, seki_2.b)
		seki_2.tmp = 0
	End Sub

	' 更新
	Public Sub update(form1_n As Integer)
		n = form1_n

		If n <> n_max Then
			calc_Gaussian()
			calc_seki_1()
			calc_seki_2()
		End If

	End Sub

	' tを取得
	Private Function get_t(a As Double, b As Double)
		Return (b - a) / n_max
	End Function

	' ガウス積分
	Private Sub calc_Gaussian()
		Dim x As Double = gi.a + gi.t * n
		gi.tmp = gi.tmp + Exp(-((x) ^ 2)) * gi.t

	End Sub

	Public Function get_Gaussian()
		Return gi.tmp ^ 2
	End Function

	' {\pi =2 \int _{-1}^{1} {\sqrt {1-t^{2}} }
	Private Sub calc_seki_1()
		Dim x As Double = seki_1.a + seki_1.t * n
		seki_1.tmp = seki_1.tmp + (Sqrt(1 - x ^ 2)) * seki_1.t
	End Sub

	Public Function get_seki_1()
		Return 2 * seki_1.tmp
	End Function

	'　{\pi =4\int _{0}^{1}{\frac {dt}{1+t^{2}}} } 
	Private Sub calc_seki_2()
		Dim x As Double = seki_2.a + seki_2.t * n
		seki_2.tmp = seki_2.tmp + 1.0 / (1 + x ^ 2) * seki_2.t
	End Sub

	Public Function get_seki_2()
		Return 4 * seki_2.tmp
	End Function

End Class
