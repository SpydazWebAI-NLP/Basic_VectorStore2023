Public Class SearchEngine
    Private documents As List(Of Document)

    Public Sub New()
        documents = New List(Of Document)()
    End Sub

    Public Sub AddDocument(id As Integer, content As String)
        documents.Add(New Document() With {.Id = id, .Content = content})
    End Sub

    Public Function Search(query As String) As List(Of Tuple(Of Integer, String))
        Dim searchResults As New List(Of Tuple(Of Integer, String))()

        For Each doc As Document In documents
            If doc.Content.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim snippet As String = ExtractContextualSnippet(doc.Content, query)
                searchResults.Add(New Tuple(Of Integer, String)(doc.Id, snippet))
            End If
        Next

        Return searchResults
    End Function

    Private Function ExtractContextualSnippet(content As String, query As String) As String
        Dim queryIndex As Integer = content.IndexOf(query, StringComparison.OrdinalIgnoreCase)

        Dim snippetStart As Integer = Math.Max(0, queryIndex - 50)
        Dim snippetEnd As Integer = Math.Min(content.Length, queryIndex + query.Length + 50)

        Dim snippet As String = content.Substring(snippetStart, snippetEnd - snippetStart)

        ' Highlight query terms in the snippet
        snippet = snippet.Replace(query, $"<strong>{query}</strong>")

        Return snippet
    End Function
End Class
Public Class Document
    Public Property Content As String
    Public Property Id As Integer
End Class
