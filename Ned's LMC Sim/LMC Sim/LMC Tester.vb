﻿'Welcome to NED'S LMC TESTER
'There really isn't much too this. Prepare to be disappointed.
Public Class LMCTester


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
        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Start up the BackgroundWorker1.

    End Sub
    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As System.Object,
                                         ByVal e As System.ComponentModel.ProgressChangedEventArgs) _
                                         Handles BackgroundWorker1.ProgressChanged
        Select Case e.ProgressPercentage
            Case <= 105
                TextBox2.AppendText(e.UserState & Environment.NewLine)
            Case 106
                TextBox2.AppendText(Environment.NewLine)
                TextBox2.AppendText(Environment.NewLine)
                TextBox2.AppendText("Mean number of cycles was " & e.UserState & Environment.NewLine)
            Case 107
                TextBox2.AppendText("Longest run was " & CInt(e.UserState))
            Case 108
                TextBox2.AppendText(" at " & e.UserState & " cycles." & Environment.NewLine)
            Case 109
                TextBox2.AppendText("The program timed out on " & e.UserState & " different cycles." & Environment.NewLine)

        End Select
    End Sub
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object,
                                         ByVal e As System.ComponentModel.DoWorkEventArgs) _
                                         Handles BackgroundWorker1.DoWork
        ' Do some time-consuming work on this thread.
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
            Dim Out As String = ""
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
                    Out = Out & (Calc & ",")
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
                    Out = Out & (" -   FAILURE - TIMED OUT")
                    Exit Do
                End If
            Loop
            If Cycles > BCycle Then
                BCycle = Cycles
                BCycle2 = inputNum
            End If
            '  Console.ReadLine()
            Out = Out & (" - In " & Cycles & " Cycles")

            progress += 0.1
            BackgroundWorker1.ReportProgress(CInt(progress), Out)

        Next
        BackgroundWorker1.ReportProgress(106, CInt(TCycles / 1000))

        BackgroundWorker1.ReportProgress(107, BCycle2)

        BackgroundWorker1.ReportProgress(108, BCycle)
        If fails > 0 Then
            BackgroundWorker1.ReportProgress(109, fails)

        End If
    End Sub

End Class