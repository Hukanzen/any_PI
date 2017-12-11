'
' いっPI計算するクラス
'

'
' 実装内容
' ・PI/4 = Σ(n=0->inf) ((-1)^n)/(2n+1) ...ライプニッツの公式
' ・ζ(4) = (PI^4)/90 ...ゼータ関数，バーゼル問題
' ・ガウス=ルジャンドルのアルゴリズム
' 'Chudonovsky
'
Imports System.Math

Public Class Sigma_Calc_PI
	' simgaの束縛変数
	Private n As Integer

	' 精度
	'Private Const Mag As Integer = 1 * 10

	' ライプニッツの公式
	Private tmp_Leibniz As Double
	' zeta(4)
	Private tmp_zeta_4 As Double

	' ガウス=ルジャンドルのアルゴリズム
	Private Structure Gauss_Legendre
		Dim a As Double
		Dim b As Double
		Dim t As Double
		Dim p As Double
	End Structure
	Private gl As Gauss_Legendre

	' Chudonovsky
	Private Structure Chudonovsky
		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim tmp As Double
	End Structure
	Private chdnv As Chudonovsky

	' コンストラクタ
	Public Sub New()
		reset()
	End Sub

	' 初期化関数
	Public Sub reset()
		tmp_Leibniz = 0
		tmp_zeta_4 = 0

		gl.a = 1.0
		gl.b = 1.0 / Sqrt(2)
		gl.t = 1.0 / 4.0
		gl.p = 1.0

		chdnv.A = 13591409.0
		chdnv.B = 545140134
		chdnv.C = 640320
		chdnv.tmp = 0

	End Sub

	' 階乗
	Private Function Factorial(x As Integer)
		'If x <= 1 Then
		'	Return x = 1
		'Else
		'	Return x * Factorial(x - 1)
		'End If

		Dim y As Double = x
		For x = x - 1 To 1 Step -1
			y = y * x
		Next x

		Return y
	End Function


	' 更新
	Public Sub update(form1_n As Integer)
		n = form1_n

		calc_Leibniz()
		calc_zeta_4()
		calc_Gauss_Legendre()
		calc_Chudonovsky()

	End Sub

	' ライプニッツの公式
	Private Sub calc_Leibniz()
		tmp_Leibniz = tmp_Leibniz + ((-1) ^ n) / (2 * n + 1)
	End Sub

	Public Function get_Leibniz()
		Return tmp_Leibniz * 4
	End Function

	' ζ(4)=PI^4/90
	Private Sub calc_zeta_4()
		tmp_zeta_4 = tmp_zeta_4 + (1 / (n + 1) ^ 4)
	End Sub

	Public Function get_zeta_4()
		Return (90 * tmp_zeta_4) ^ (1 / 4)
	End Function

	' ガウス=ルジャンドルのアルゴリズム
	Private Sub calc_Gauss_Legendre()
		' a_nを保存
		Dim gla_n As Double = gl.a

		gl.a = (gl.a + gl.b) / 2
		gl.b = Sqrt(gla_n * gl.b)
		gl.t = gl.t - gl.p * (gla_n - gl.a) ^ 2
		gl.p = 2 * gl.p

	End Sub

	Public Function get_Gauss_Legendre()
		Return (gl.a + gl.b) ^ 2 / (4 * gl.t)
	End Function

	' Chudonovsky
	' 複雑だよ
	Private Sub calc_Chudonovsky()
		Dim child As Double
		Dim mather As Double

		child = (-1) ^ n * Factorial(6 * n) * (chdnv.A + chdnv.B * n)
		mather = Factorial(3 * n) * (Factorial(n) ^ 3) * chdnv.C ^ (3 * n)

		chdnv.tmp = chdnv.tmp + child / mather
	End Sub

	Public Function get_Chudonovsky()
		Dim invs As Double
		invs = 12.0 / Sqrt(chdnv.C ^ 3) * chdnv.tmp
		Return 1.0 / invs
	End Function

End Class

'========================
'   EOF Sigma_Calc_PI.vb
'========================