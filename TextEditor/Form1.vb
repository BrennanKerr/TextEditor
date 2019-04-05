' TextEditor - An application that allows text to be edited
' Brennan Kerr
' February 12th, 2019

Option Strict On

Imports System.IO   ' allows opening, editing, and saving of files
' FileSteam, StreamReader, StreamWriter are included in this namespace

Public Class frmEditor
	Dim path As String = ""             ' the path of the opened file
	Dim previousText As String = ""     ' the text of the file at last save point/opening
	Dim currentFile As FileStream       ' the stream of the file
	Dim reader As StreamReader          ' allows the file to be read (depending on permission)
	Dim writer As StreamWriter          ' allows writing to the file (Depending on permissions)

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

	End Sub

	' AboutApplication - displays the information about the application
	Private Sub AboutApplication(sender As Object, e As EventArgs) Handles mnuAbout.Click
		MessageBox.Show("NETD-2202" + vbCrLf + "Lab # 5" + vbCrLf + "Brennan Kerr", "About")
	End Sub

	' ExitApplication - if the user wants to exit the application
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

	' OpenFile - if the user wants to open a file
	Private Sub OpenFile(sender As Object, e As EventArgs) Handles mnuOpen.Click
		Try
			' checks to see if the file is available
			If CheckText() = True Then
				ChooseFile(sender, e)
			Else
				SaveFileAs(sender, e)
				ChooseFile(sender, e)
			End If
		Catch ex As Exception
			Console.WriteLine(ex)
		End Try
	End Sub

	' ChooseFile - allows the user to select a file to open
	Private Sub ChooseFile(sender As Object, e As EventArgs)
		If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
			path = OpenFileDialog1.FileName
			currentFile = New FileStream(path, FileMode.Open, FileAccess.ReadWrite)
			reader = New StreamReader(currentFile)

			tbInput.Text = reader.ReadToEnd

			previousText = tbInput.Text

			reader.Close()

			Text = "Text Editor: " + path + " Open"
		End If
	End Sub

	' SaveFile - if the user wants to save the file
	Private Sub SaveFile(sender As Object, e As EventArgs) Handles mnuSave.Click
		Try
			' if the file already exists
			If File.Exists(path) Then
				currentFile = New FileStream(path, FileMode.Create, FileAccess.Write)
				writer = New StreamWriter(currentFile)

				writer.Write(tbInput.Text)
				writer.Close()

				previousText = tbInput.Text
				' if the file does not exists (it is a new file), goes to the SaveFileAs method
			Else
				SaveFileAs(sender, e)
			End If
		Catch ex As Exception
			Console.WriteLine(ex)
		End Try
	End Sub

	' SaveFileAs - if the user wants to save the file as something else
	Private Sub SaveFileAs(sender As Object, e As EventArgs) Handles mnuSaveAs.Click
		SaveFileDialog1.Filter = "TXT Files (*.txt*)|*.txt"
		Try
			If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
				path = SaveFileDialog1.FileName
				My.Computer.FileSystem.WriteAllText(path, tbInput.Text, False)
				previousText = tbInput.Text
			End If
		Catch ex As Exception
			Console.WriteLine(ex)
		End Try
	End Sub

	' NewFile - if the user wants to create a new file
	Private Sub NewFile(sender As Object, e As EventArgs) Handles mnuNew.Click

		' checks to see if the text hasn't been changed
		If CheckText() = True Then
			CreateBlankFile()
			' if the text has changed
		Else
			SaveFileAs(sender, e)
			CreateBlankFile()
		End If
	End Sub

	' CreateBlankFile - creates a blank file when a new file is requested.
	Private Sub CreateBlankFile()
		' resets the values and removes all text
		path = ""
		tbInput.Text = ""
		previousText = ""

		' changes the title bar
		Text = "Text Editor: Select a File to Open"
	End Sub

	' CutText - cremoves the text from the screen and saves it to the clipboard
	Private Sub CutText(sender As Object, e As EventArgs) Handles mnuCut.Click
		' cuts the text
		tbInput.Cut()
	End Sub

	' CopyText - saves the text to the clipboard
	Private Sub CopyTest(sender As Object, e As EventArgs) Handles mnuCopy.Click
		tbInput.Copy()
	End Sub

	' PasteText - put whatever text is in the clipboard to the desired location
	Private Sub PasteText(sender As Object, e As EventArgs) Handles mnuPaste.Click
		' gets the copied text
		tbInput.Paste()
	End Sub

	' CheckText - checks to see if the text has been altered since it was opened
	' @return	state : boolean - true if it has not been altered, false if it has
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

	'' CloseFile - Closes the currently opened file
	Private Sub CloseFile(sender As Object, e As EventArgs) Handles mnuClose.Click
		Try
			' closes and opens a new file
			NewFile(sender, e)
		Catch ex As Exception
			Console.WriteLine(ex)
		End Try
	End Sub
End Class