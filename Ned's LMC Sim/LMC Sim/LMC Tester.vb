'Welcome to NED'S LMC TESTER
'There really isn't much too this. Prepare to be disappointed.
Public Class LMCTester
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Nothing happens here
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.Title = "Select a .LMC file"


        OpenFileDialog1.InitialDirectory = "C:temp"


        OpenFileDialog1.ShowDialog()
    End Sub
    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk


        Dim strm As System.IO.Stream


        strm = OpenFileDialog1.OpenFile()


        TextBox1.Text = OpenFileDialog1.FileName.ToString()


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim FILE_NAME As String = TextBox1.Text

        Dim TextLine As String = ""

        If System.IO.File.Exists(FILE_NAME) = True Then

            Dim objReader As New System.IO.StreamReader(FILE_NAME)

            Do While objReader.Peek() <> -1

                TextLine = TextLine & objReader.ReadLine() & vbNewLine

            Loop



        Else

            TextBox2.AppendText("File Does Not Exist")

        End If

        Dim TP2 = TextLine.Split("%")
        Dim code() As String = Mid(TP2(2), 1, Len(TP2(2)) - 1).Split(",")


        Dim progress As Decimal = 0
        Dim Input(100) As Integer
        Console.WriteLine("Enter LMC machine code:")


        For i = 0 To code.Length - 1
            Input(i) = CInt(code(i))
        Next
        Dim negative As Boolean = False
        Dim fails As Integer = 0
        Dim TCycles As Integer = 0
        Dim BCycle As Integer = 0
        Dim BCycle2 As Integer = 0
        For inputNum = 0 To 999
            Dim Calc As Integer = 0
            Dim Cycles = 0
            Dim Counter As Integer = 0
            Do Until Input(Counter) < 100
                Dim a = Input(Counter)

                If (a - (a Mod 100)) = 100 Then
                    Calc += Input(a Mod 100)
                ElseIf (a - (a Mod 100)) = 200 Then
                    Calc -= Input(a Mod 100)
                ElseIf (a - (a Mod 100)) = 300 Then
                    Input(a Mod 100) = Calc
                ElseIf (a - (a Mod 100)) = 500 Then
                    Calc = Input(a Mod 100)
                    negative = False
                ElseIf (a - (a Mod 100)) = 600 Then
                    Counter = (a Mod 100) - 1
                ElseIf (a - (a Mod 100)) = 700 And Calc = 0 Then
                    Counter = (a Mod 100) - 1
                ElseIf (a - (a Mod 100)) = 800 And negative = False Then
                    Counter = (a Mod 100) - 1
                ElseIf a = 901 Then
                    Calc = inputNum
                    negative = False
                ElseIf a = 902 Then
                    TextBox2.AppendText(Calc & ",")

                End If
                If Calc >= 1000 Then
                    Calc = Calc Mod 1000
                ElseIf Calc < 0 Then
                    Calc = (Calc + 10000) Mod 1000
                    negative = True
                End If
                Counter += 1
                Cycles += 1
                TCycles += 1
                If Cycles > 100000 Then
                    fails += 1
                    TextBox2.AppendText(Environment.NewLine & "FAILURE - TIMED OUT" & Environment.NewLine)
                    Exit Do
                End If
            Loop
            If Cycles > BCycle Then
                BCycle = Cycles
                BCycle2 = inputNum
            End If
            '  Console.ReadLine()
            TextBox2.AppendText(" - In " & Cycles & " Cycles" & Environment.NewLine)

            progress += 0.1
            ProgressBar1.Value = CInt(progress)
        Next
        TextBox2.AppendText(Environment.NewLine)
        TextBox2.AppendText(Environment.NewLine)
        TextBox2.AppendText("Mean number of cycles was " & CInt(TCycles / 1000) & Environment.NewLine)
        TextBox2.AppendText("Longest run was " & CInt(BCycle2) & " at " & BCycle & " cycles." & Environment.NewLine)
        If fails > 0 Then
            TextBox2.AppendText("The program timed out on " & fails & " different cycles." & Environment.NewLine)
        End If
    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs) Handles ProgressBar1.Click

    End Sub
End Class