' TextEditor - An application that allows text to be edited
' Brennan Kerr
' February 12th, 2019


Option Strict On

Imports System.IO

Public Class frmEditor
	Dim path As String = ""
	Dim previousText As String = ""
	Dim fileRead As FileStream
	Dim fileWrite As FileStream
	Dim reader As StreamReader
	Dim writer As StreamWriter

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

	End Sub

	' displays the information about the application
	Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
		MessageBox.Show("NETD-2202" + vbCrLf + "Lab # 5" + vbCrLf + "Brennan Kerr", "About")
	End Sub

	' if the user wants to exit the application
	Private Sub ExitApplication(sender As Object, e As EventArgs) Handles mnuExit.Click
		' checks to see if the text hasnt changed
		If CheckText() = True Then
			Close()
			' if the text has changed
		Else
			SaveFileAs(sender, e)
			Close()
		End If
	End Sub

	' if the user wants to open a file
	Private Sub OpenFile(sender As Object, e As EventArgs) Handles mnuOpen.Click
		' checks to see if the file is available
		If CheckText() = True Then
			If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
				path = OpenFileDialog1.FileName
				fileRead = New FileStream(path, FileMode.Open, FileAccess.ReadWrite)
				reader = New StreamReader(fileRead)

				tbInput.Text = reader.ReadToEnd

				previousText = tbInput.Text

				reader.Close()

				Text = "Text Editor: " + path + " Open"
			End If
		Else
			SaveFileAs(sender, e)
			OpenFile(sender, e)
		End If
	End Sub

	' if the user wants to save the file
	Private Sub SaveFile(sender As Object, e As EventArgs) Handles mnuSave.Click
		' if the file already exists
		If File.Exists(path) Then
			fileWrite = New FileStream(path, FileMode.Create, FileAccess.Write)
			writer = New StreamWriter(fileWrite)

			writer.Write(tbInput.Text)
			writer.Close()
			' if the file does not exists (it is a new file), goes to the SaveFileAs method
		Else
			SaveFileAs(sender, e)
		End If

		previousText = tbInput.Text
	End Sub

	' if the user wants to save the file as something else
	Private Sub SaveFileAs(sender As Object, e As EventArgs) Handles mnuSaveAs.Click
		If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
			path = SaveFileDialog1.FileName
			My.Computer.FileSystem.WriteAllText(path, tbInput.Text, False)
		End If

		previousText = tbInput.Text
	End Sub

	' if the user wants to create a new file
	Private Sub NewFile(sender As Object, e As EventArgs) Handles mnuNew.Click

		' checks to see if the text hasn't been changed
		If CheckText() = True Then
			' resets the values and removes all text
			path = ""
			tbInput.Text = ""

			' changes the title bar
			Text = "Text Editor: Select a File to Open"

			' if the text has changed
		Else
			SaveFileAs(sender, e)
			NewFile(sender, e)
		End If
	End Sub

	' cuts the text
	Private Sub CutText(sender As Object, e As EventArgs) Handles mnuCut.Click
		' cuts the text
		tbInput.Cut()
	End Sub

	' if the user wants to copy text
	Private Sub CopyTest(sender As Object, e As EventArgs) Handles mnuCopy.Click
		tbInput.Copy()
	End Sub

	' if the user wants to past text
	Private Sub PasteText(sender As Object, e As EventArgs) Handles mnuPaste.Click
		' gets the copied text
		tbInput.Paste()
	End Sub

	' if the text has been altered
	Private Function CheckText() As Boolean
		Dim state As Boolean

		' checks to see if the text has been altered
		If previousText = tbInput.Text Then
			state = True
		Else
			state = False
		End If

		Return state
	End Function

End Class