Imports System.Reflection

Namespace Common

    Public Class Utils
        Public Shared Sub TransferDataFromObject(sourceObject As Object, targetObject As Object)

            For Each p As PropertyInfo In sourceObject.GetType().GetProperties()
                Dim k As PropertyInfo = targetObject.GetType().GetProperty(p.Name)
                If k IsNot Nothing Then
                    If Convert.GetTypeCode(p.GetValue(sourceObject)) <> TypeCode.Object Then
                        k.SetValue(targetObject, p.GetValue(sourceObject))
                    Else
                        Dim newInstance As Object
                        newInstance = Activator.CreateInstance(k.PropertyType)
                        k.SetValue(targetObject, newInstance)
                        TransferDataFromObject(p.GetValue(sourceObject), newInstance)
                    End If
                End If
            Next

        End Sub

    End Class

End Namespace
