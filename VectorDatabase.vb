Imports System.Numerics
Imports MathNet.Numerics
Public Class VectorDatabase
    Private AudioVectors As Dictionary(Of Integer, List(Of Complex)) = New Dictionary(Of Integer, List(Of Complex))()
    Private ImageVectors As Dictionary(Of Integer, Tuple(Of VectorType, List(Of Double))) = New Dictionary(Of Integer, Tuple(Of VectorType, List(Of Double)))()
    Private TextVectors As Dictionary(Of Integer, List(Of Double)) = New Dictionary(Of Integer, List(Of Double))()
    Public Enum VectorType
        Text
        Image
        Audio
    End Enum

    Public Sub AddAudioVector(id As Integer, vector As List(Of Complex))
        AudioVectors.Add(id, vector)
    End Sub
    Public Sub AddImageVector(id As Integer, vector As List(Of Double))
        ImageVectors.Add(id, Tuple.Create(VectorType.Image, vector))
    End Sub
    Public Sub AddTextVector(id As Integer, vector As List(Of Double))
        ImageVectors.Add(id, Tuple.Create(VectorType.Text, vector))
    End Sub
    Public Sub AddVector(id As Integer, vector As List(Of Double), vectorType As VectorType)
        If vectorType = VectorType.Text Then

            TextVectors.Add(id, vector)
        ElseIf vectorType = VectorType.Image Then
            ImageVectors.Add(id, Tuple.Create(VectorType.Image, vector))

        End If
    End Sub
    Public Sub AddVector(id As Integer, vector As List(Of Complex), vectorType As VectorType)
        If vectorType = VectorType.Audio Then
            AudioVectors.Add(id, vector)
        End If
    End Sub
    Public Function FindSimilarAudioVectors(queryVector As List(Of Complex), numNeighbors As Integer) As List(Of Integer)
        Dim similarVectors As List(Of Integer) = New List(Of Integer)()

        For Each vectorId As Integer In AudioVectors.Keys
            Dim vectorData As List(Of Complex) = AudioVectors(vectorId)
            Dim distance As Double = CalculateEuclideanDistanceComplex(queryVector, vectorData)

            ' Maintain a list of the closest neighbors
            If similarVectors.Count < numNeighbors Then
                similarVectors.Add(vectorId)
            Else
                Dim maxDistance As Double = GetMaxDistanceComplex(similarVectors, queryVector)
                If distance < maxDistance Then
                    Dim indexToRemove As Integer = GetIndexOfMaxDistanceComplex(similarVectors, queryVector)
                    similarVectors.RemoveAt(indexToRemove)
                    similarVectors.Add(vectorId)
                End If
            End If
        Next

        Return similarVectors
    End Function
    Public Function FindSimilarImageVectors(queryVector As List(Of Double), numNeighbors As Integer) As List(Of Integer)
        Dim similarVectors As List(Of Integer) = New List(Of Integer)

        For Each vectorId As Integer In ImageVectors.Keys
            Dim vectorType As VectorType
            Dim vectorData As List(Of Double)
            Dim vectorTuple As Tuple(Of VectorType, List(Of Double)) = ImageVectors(vectorId)
            vectorType = vectorTuple.Item1
            vectorData = vectorTuple.Item2

            If vectorType = VectorType.Image Then
                ' Calculate similarity using image-specific logic
                ' You can integrate the image similarity logic here
            ElseIf vectorType = VectorType.Text Then
                ' Calculate similarity using text-specific logic
                Dim distance As Double = CalculateEuclideanDistance(queryVector, vectorData)

                ' Maintain a list of the closest neighbors
                If similarVectors.Count < numNeighbors Then
                    similarVectors.Add(vectorId)
                Else
                    Dim maxDistance As Double = GetMaxDistance(similarVectors, queryVector)
                    If distance < maxDistance Then
                        Dim indexToRemove As Integer = GetIndexOfMaxDistanceImages(similarVectors, queryVector)
                        similarVectors.RemoveAt(indexToRemove)
                        similarVectors.Add(vectorId)
                    End If
                End If
            End If
        Next

        Return similarVectors
    End Function
    Public Function FindSimilarTextVectors(queryVector As List(Of Double), numNeighbors As Integer) As List(Of Integer)
        Dim similarVectors As List(Of Integer) = New List(Of Integer)()

        For Each vectorId As Integer In TextVectors.Keys
            Dim vector As List(Of Double) = TextVectors(vectorId)
            Dim distance As Double = CalculateEuclideanDistance(queryVector, vector)

            ' Maintain a list of the closest neighbors
            If similarVectors.Count < numNeighbors Then
                similarVectors.Add(vectorId)
            Else
                Dim maxDistance As Double = GetTextVectorsMaxDistance(similarVectors, queryVector)
                If distance < maxDistance Then
                    Dim indexToRemove As Integer = GetIndexOfMaxDistance(similarVectors, queryVector)
                    similarVectors.RemoveAt(indexToRemove)
                    similarVectors.Add(vectorId)
                End If
            End If
        Next

        Return similarVectors
    End Function
    Private Function CalculateEuclideanDistance(vector1 As List(Of Double), vector2 As List(Of Double)) As Double
        Dim sum As Double = 0
        For i As Integer = 0 To vector1.Count - 1
            sum += Math.Pow(vector1(i) - vector2(i), 2)
        Next
        Return Math.Sqrt(sum)
    End Function
    Private Function CalculateEuclideanDistanceComplex(vector1 As List(Of Complex), vector2 As List(Of Complex)) As Double
        Dim sum As Double = 0
        For i As Integer = 0 To vector1.Count - 1
            Dim difference As Complex = vector1(i) - vector2(i)
            sum += difference.MagnitudeSquared
        Next
        Return Math.Sqrt(sum)
    End Function
    Private Function GetIndexOfMaxDistance(vectorIds As List(Of Integer), queryVector As List(Of Double)) As Integer
        Dim maxDistance As Double = Double.MinValue
        Dim indexToRemove As Integer = -1
        For i As Integer = 0 To vectorIds.Count - 1
            Dim vectorId As Integer = vectorIds(i)
            Dim vector As List(Of Double) = TextVectors(vectorId)
            Dim distance As Double = CalculateEuclideanDistance(queryVector, vector)
            If distance > maxDistance Then
                maxDistance = distance
                indexToRemove = i
            End If
        Next
        Return indexToRemove
    End Function
    Private Function GetIndexOfMaxDistanceComplex(vectorIds As List(Of Integer), queryVector As List(Of Complex)) As Integer
        Dim maxDistance As Double = Double.MinValue
        Dim indexToRemove As Integer = -1
        For i As Integer = 0 To vectorIds.Count - 1
            Dim vectorId As Integer = vectorIds(i)
            Dim vectorData As List(Of Complex) = AudioVectors(vectorId)
            Dim distance As Double = CalculateEuclideanDistanceComplex(queryVector, vectorData)
            If distance > maxDistance Then
                maxDistance = distance
                indexToRemove = i
            End If
        Next
        Return indexToRemove
    End Function
    Private Function GetIndexOfMaxDistanceImages(vectorIds As List(Of Integer), queryVector As List(Of Double)) As Integer
        Dim maxDistance As Double = Double.MinValue
        Dim indexToRemove As Integer = -1
        For i As Integer = 0 To vectorIds.Count - 1
            Dim vectorId As Integer = vectorIds(i)
            Dim vector = ImageVectors(vectorId)


            Dim distance As Double = CalculateEuclideanDistance(queryVector, vector.Item2)
            If distance > maxDistance Then
                maxDistance = distance
                indexToRemove = i
            End If
        Next
        Return indexToRemove
    End Function
    Private Function GetMaxDistance(vectorIds As List(Of Integer), queryVector As List(Of Double)) As Double
        Dim maxDistance As Double = Double.MinValue
        For Each vectorId As Integer In vectorIds
            Dim vectorType As VectorType
            Dim vectorData As List(Of Double)
            Dim vectorTuple As Tuple(Of VectorType, List(Of Double)) = ImageVectors(vectorId)
            vectorType = vectorTuple.Item1
            vectorData = vectorTuple.Item2

            If vectorType = VectorType.Text Then
                Dim distance As Double = CalculateEuclideanDistance(queryVector, vectorData)
                If distance > maxDistance Then
                    maxDistance = distance
                End If
            End If
        Next
        Return maxDistance
    End Function
    Private Function GetMaxDistanceComplex(vectorIds As List(Of Integer), queryVector As List(Of Complex)) As Double
        Dim maxDistance As Double = Double.MinValue
        For Each vectorId As Integer In vectorIds
            Dim vectorData As List(Of Complex) = AudioVectors(vectorId)
            Dim distance As Double = CalculateEuclideanDistanceComplex(queryVector, vectorData)
            If distance > maxDistance Then
                maxDistance = distance
            End If
        Next
        Return maxDistance
    End Function
    Private Function GetTextVectorsMaxDistance(vectorIds As List(Of Integer), queryVector As List(Of Double)) As Double
        Dim maxDistance As Double = Double.MinValue
        For Each vectorId As Integer In vectorIds
            Dim vector As List(Of Double) = TextVectors(vectorId)
            Dim distance As Double = CalculateEuclideanDistance(queryVector, vector)
            If distance > maxDistance Then
                maxDistance = distance
            End If
        Next
        Return maxDistance
    End Function
End Class
