
Module Module1

    Sub Main()

        Dim Input(100) As Integer
        Console.WriteLine("Enter LMC machine code:")
        Dim code() As String = Console.ReadLine().Split(",")

        For i = 0 To code.Length - 1
            Input(i) = CInt(code(i))
            Console.WriteLine(Input(i))
        Next
        Console.ReadLine()
        Dim negative As Boolean
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
                ElseIf (a - (a Mod 100)) = 600 Then
                    Counter = (a Mod 100) - 1
                ElseIf (a - (a Mod 100)) = 700 And Calc = 0 Then
                    Counter = (a Mod 100) - 1
                ElseIf (a - (a Mod 100)) = 800 And Calc > 0 Then
                    Counter = (a Mod 100) - 1
                ElseIf a = 901 Then
                    Calc = inputNum
                ElseIf a = 902 Then
                    Console.Write(Calc & ",")

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

            Loop
            If Cycles > BCycle Then
                BCycle = Cycles
                BCycle2 = inputNum
            End If
            '  Console.ReadLine()
            Console.WriteLine(" - In " & Cycles & " Cycles")
        Next
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("Mean number of cycles was " & CInt(TCycles / 1000))
        Console.WriteLine("Longest run was " & CInt(BCycle2) & " at " & BCycle & " cycles.")
        Console.ReadLine()
    End Sub

End Module
