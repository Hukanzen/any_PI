' モードレス

Public Class Monte_Carlo_Circle_Form
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

	' グラフ関係のクラス
	Private g_graph As Graphics



	Private Sub Monte_Carlo_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		' グラフように変数初期化
		g_graph = PictureBox1.CreateGraphics
	End Sub

	'====================================================
	' 初期化
	'====================================================
	'
	' PictureBox1をグラフに初期化する
	'
	Public Sub graph_init(def_x_min As Double, def_x_max As Double, def_y_min As Double, def_y_max As Double)

		def_x.min = def_x_min
		def_x.max = def_x_max
		def_y.min = def_y_min
		def_y.max = def_y_max

		Dim i As Integer
		Dim j As Integer

		'PictureBox1を白色で塗りつぶす
		g_graph.FillRectangle(Brushes.White, New Rectangle(0, 0, PictureBox1.Width, PictureBox1.Height))
		'原点のグラフィイク座標を求め，座標軸を描画する
		xy2ij(0, 0, i, j, def_x.min, def_x.max, def_y.min, def_y.max, PictureBox1.Width, PictureBox1.Height)
		g_graph.DrawLine(Pens.Black, i, 0, i, PictureBox1.Height - 1)
		g_graph.DrawLine(Pens.Black, 0, j, PictureBox1.Width - 1, j)

		' 目盛り
		Dim font As New Font("MS UI Gothic", 10)

		Dim x As Double, y
		For x = def_x.min To def_x.max Step 0.1
			If Not (x Mod 10) Then
				xy2ij(x, 0, i, j, def_x.min, def_x.max, def_y.min, def_y.max, PictureBox1.Width, PictureBox1.Height)
				g_graph.DrawString(x, font, Brushes.Gray, i, 0)
				g_graph.DrawLine(Pens.Gray, i, 0, i, PictureBox1.Height - 1)
			End If

		Next x

		For y = def_y.min To def_y.max Step 0.1
			If Not (y Mod 10) Then
				xy2ij(0, y, i, j, def_x.min, def_x.max, def_y.min, def_y.max, PictureBox1.Width, PictureBox1.Height)
				g_graph.DrawString(y, font, Brushes.Gray, 0, j)
				g_graph.DrawLine(Pens.Gray, 0, j, PictureBox1.Width - 1, j)
			End If
		Next

		'y=PIの赤線
		xy2ij(0, System.Math.PI, i, j, def_x.min, def_x.max, def_y.min, def_y.max, PictureBox1.Width, PictureBox1.Height)
		g_graph.DrawLine(Pens.Red, 0, j, PictureBox1.Width - 1, j)
	End Sub


	'====================================================
	' 自作関数
	'====================================================
	'
	'xy2ij : 座標変換(x,y)->(i,j)
	'実座標（論理座標）(x,y)からグラフィック座標(i,j)を求める
	'実行後(i,j)に値が代入される。（i,jは参照渡し）
	'
	Private Sub xy2ij(x As Double, y As Double, ByRef i As Integer, ByRef j As Integer, x_min As Double, x_max As Double, y_min As Double, y_max As Double, Width As Integer, Height As Integer)
		i = (Width - 1) * (x - x_min) / (x_max - x_min)
		j = (Height - 1) * (y_max - y) / (y_max - y_min)
	End Sub

	'
	' グラフのdotを打つ
	'
	Public Sub graph_dot(x As Double, y As Double, cr As Integer, color As Pen)
		Dim i As Integer
		Dim j As Integer
		xy2ij(x, y, i, j, def_x.min, def_x.max, def_y.min, def_y.max, PictureBox1.Width, PictureBox1.Height)
		g_graph.DrawEllipse(color, New Rectangle(i, j, cr, cr))
	End Sub
End Class