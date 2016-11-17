Imports System.Web.Mvc
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Http.Formatting
Imports System.Configuration
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports CadastroMedicos.Domain

Namespace Controllers
    Public Class CrudController(Of TDtoObject As DataTransferObject)
        Inherits Controller

        Private _controllerName As String

        Public Sub New(controllerName As String)
            _controllerName = controllerName
        End Sub

        <HttpGet>
        Public Async Function Index() As Task(Of ActionResult)
            Dim dtoResult As IList(Of TDtoObject)

            Using client As New HttpClient()
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                Using response As HttpResponseMessage = Await client.GetAsync(_controllerName)
                    If response.IsSuccessStatusCode Then
                        Dim jsonResult As String
                        jsonResult = Await response.Content.ReadAsStringAsync()
                        dtoResult = JsonConvert.DeserializeObject(Of TDtoObject())(jsonResult).ToList
                    End If
                End Using
            End Using

            Return View(dtoResult)
        End Function

        <HttpGet>
        Public Function Create() As ActionResult
            Return View()
        End Function

        <HttpPost>
        Public Async Function Create(dto As TDtoObject) As Task(Of ActionResult)
            Dim uri As String
            Dim jsonResult As String
            Dim content As StringContent

            uri = String.Format("{0}",_controllerName)

            Using client As New HttpClient
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                jsonResult = JsonConvert.SerializeObject(dto)
                content = New StringContent(jsonResult, Encoding.UTF8, "application/json")

                Using response As HttpResponseMessage = Await client.PostAsync(uri, content)
                    If response.IsSuccessStatusCode Then
                        Return RedirectToAction("Index")
                    Else
                        ModelState.AddModelError(String.Empty, "Erro ao processar solicitar a requisição")
                        Return View(dto)
                    End If
                End Using
            End Using

        End Function

        <HttpGet>
        Public Async Function Edit(id As Long) As Task(Of ActionResult)
            Dim dtoResult As TDtoObject
            Dim uri As String

            uri = String.Format("{0}/{1}", _controllerName, id)

            Using client As New HttpClient
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                Using response As HttpResponseMessage = Await client.GetAsync(uri)
                    If response.IsSuccessStatusCode Then
                        Dim jsonResult As String
                        jsonResult = Await response.Content.ReadAsStringAsync()
                        dtoResult = JsonConvert.DeserializeObject(Of TDtoObject)(jsonResult)
                    Else
                        ModelState.AddModelError(String.Empty, "Erro ao processar solicitar a requisição")
                    End If
                End Using
            End Using

            Return View(dtoResult)
        End Function

        <HttpPost>
        Public Async Function Edit(dto As TDtoObject) As Task(Of ActionResult)

            Dim uri As String
            Dim jsonResult As String
            Dim content As StringContent

            uri = String.Format("{0}/{1}", _controllerName, dto.Id)

            Using client As New HttpClient
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                jsonResult = JsonConvert.SerializeObject(dto)
                content = New StringContent(jsonResult, Encoding.UTF8, "application/json")

                Using response As HttpResponseMessage = Await client.PutAsync(uri, content)
                    If response.IsSuccessStatusCode Then
                        Return RedirectToAction("Index")
                    Else
                        ModelState.AddModelError(String.Empty, "Erro ao processar solicitar a requisição")
                        Return View(dto)
                    End If
                End Using
            End Using
        End Function


        <HttpGet>
        Public Async Function Details(id As Long) As Task(Of ActionResult)

            Dim dtoResult As TDtoObject
            Dim uri As String

            uri = String.Format("{0}/{1}", _controllerName, id)

            Using client As New HttpClient
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                Using response As HttpResponseMessage = Await client.GetAsync(uri)
                    If response.IsSuccessStatusCode Then
                        Dim jsonResult As String
                        jsonResult = Await response.Content.ReadAsStringAsync()
                        dtoResult = JsonConvert.DeserializeObject(Of TDtoObject)(jsonResult)
                    Else
                        ModelState.AddModelError(String.Empty, "Erro ao processar solicitar a requisição")
                    End If
                End Using
            End Using

            Return View(dtoResult)
        End Function

        <HttpGet>
        Public Async Function Delete(id As Long) As Task(Of ActionResult)
            Dim dtoResult As TDtoObject
            Dim uri As String

            uri = String.Format("{0}/{1}", _controllerName, id)

            Using client As New HttpClient
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                Using response As HttpResponseMessage = Await client.GetAsync(uri)
                    If response.IsSuccessStatusCode Then
                        Dim jsonResult As String
                        jsonResult = Await response.Content.ReadAsStringAsync()
                        dtoResult = JsonConvert.DeserializeObject(Of TDtoObject)(jsonResult)
                    Else
                        ModelState.AddModelError(String.Empty, "Erro ao processar solicitar a requisição")
                    End If
                End Using
            End Using

            Return View(dtoResult)
        End Function

        <HttpPost>
        <ActionName("Delete")>
        Public Async Function ConfirmDelete(dto As TDtoObject) As Task(Of ActionResult)

            Dim uri As String
            Dim jsonResult As String
            Dim content As StringContent

            uri = String.Format("{0}/{1}", _controllerName, dto.Id)

            Using client As New HttpClient
                client.BaseAddress = New Uri(ConfigurationManager.AppSettings("ServiceURI"))
                jsonResult = JsonConvert.SerializeObject(dto)
                content = New StringContent(jsonResult, Encoding.UTF8, "application/json")

                Using response As HttpResponseMessage = Await client.DeleteAsync(uri)
                    If response.IsSuccessStatusCode Then
                        Return RedirectToAction("Index")
                    Else
                        ModelState.AddModelError(String.Empty, "Erro ao processar solicitar a requisição")
                        Return View(dto)
                    End If
                End Using
            End Using


        End Function

    End Class
End Namespace