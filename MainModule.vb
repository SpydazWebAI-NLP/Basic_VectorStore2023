
Imports System.Numerics


Public Module MainModule
    Public Sub SaveVectorToFile(imgVector As Double(), outputPath As String)
        Using writer As New System.IO.StreamWriter(outputPath)
            For Each value As Double In imgVector
                writer.WriteLine(value)
            Next
        End Using
    End Sub
    Sub Main()
        Dim db As VectorDatabase = New VectorDatabase()

        ' Adding sample vectors to the database
        db.AddVector(1, New List(Of Double)() From {1.0, 2.0}, VectorDatabase.VectorType.Text)
        db.AddVector(2, New List(Of Double)() From {3.0, 4.0}, VectorDatabase.VectorType.Text)
        db.AddVector(3, New List(Of Double)() From {5.0, 6.0}, VectorDatabase.VectorType.Text)

        ' Query vector
        Dim queryVector As List(Of Double) = New List(Of Double)() From {2.5, 3.5}

        ' Find similar vectors
        Dim similarVectors As List(Of Integer) = db.FindSimilarTextVectors(queryVector, 2)

        ' Display results
        Console.WriteLine("Query Vector: " & String.Join(",", queryVector))
        Console.WriteLine("Similar Vectors: " & String.Join(",", similarVectors))

        Dim searchEngine As New SearchEngine()

        searchEngine.AddDocument(1, "This is an example document containing some sample text.")
        searchEngine.AddDocument(2, "Another document with additional content for demonstration.")

        Dim query As String = "example document"
        Dim results As List(Of Tuple(Of Integer, String)) = searchEngine.Search(query)

        For Each result As Tuple(Of Integer, String) In results
            Console.WriteLine($"Document ID: {result.Item1}")
            Console.WriteLine($"Snippet: {result.Item2}")
            Console.WriteLine()
        Next


        Dim imageEncoder As New Image2Vec.ImageEncoder()
        Dim imageDecoder As New Image2Vec.ImageDecoder()

        Dim imagePath As String = "path_to_your_image.jpg"
        Dim width As Integer = 224
        Dim height As Integer = 224
        Dim outputVectorPath As String = "encoded_image_vector.txt"
        Dim outputImagePath As String = "decoded_image.jpg"

        ' Encode image to vector
        Dim imgVector As Double() = imageEncoder.EncodeImage(imagePath, width, height)

        ' Save encoded vector to file
        SaveVectorToFile(imgVector, outputVectorPath)

        ' Decode vector back to image
        imageDecoder.DecodeImage(imgVector, width, height, outputImagePath)

        Console.WriteLine("Image encoded, vector saved, and image decoded.")

        Dim imageSearch As New Image2Vec.ImageSearch()

        Dim queryImagePath As String = "query_image.jpg"
        Dim ImagequeryVector As Double() = imageEncoder.EncodeImage(queryImagePath, 224, 224)

        Dim imageVectors As New List(Of Tuple(Of String, Double()))()
        ' Populate imageVectors with the names and vectors of your images

        Dim numResults As Integer = 5
        Dim similarImageNames As List(Of String) = imageSearch.FindSimilarImages(ImagequeryVector, imageVectors, numResults)

        Console.WriteLine("Similar images:")
        For Each imageName As String In similarImageNames
            Console.WriteLine(imageName)
        Next
        ' Load audio data (replace with your own audio loading code)
        Dim audioPath As String = "path_to_your_audio_file.wav"
        Dim audioSignal As Double() = Audio2Vec.LoadAudio(audioPath)

        ' Set window size and hop size for feature extraction
        Dim windowSize As Integer = 1024
        Dim hopSize As Integer = 256

        ' Convert audio to vectors
        Dim audioVectors As List(Of Complex()) = Audio2Vec.AudioToVector(audioSignal, windowSize, hopSize)

        ' Convert vectors back to audio
        Dim reconstructedAudio As Double() = Audio2Vec.VectorToAudio(audioVectors, hopSize)

        ' Save reconstructed audio (replace with your own saving code)
        Dim outputAudioPath As String = "reconstructed_audio.wav"
        Audio2Vec.SaveAudio(reconstructedAudio, outputAudioPath)

        Console.WriteLine("Audio-to-Vector and Vector-to-Audio Conversion Complete.")

    End Sub
End Module

