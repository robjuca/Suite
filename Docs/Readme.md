![](media/73b1518089c9bf2b6b6d0dec5b38286f.jpg)

*Projeto*: Suite

*Objetivo*:

Produzir uma lista vertical de gadgets (documento, imagem, vídeo etc.)

arranjados em uma lista horizontal e salvar o resultado em um banco de dados.

A ideia surgiu do site da Microsoft (figura abaixo)

![](media/1dfbcb5311ca040aef14ec476ce203ba.jpg)

Criar aplicativos (WPF) para gerenciar gadgets e fazer o layout dos mesmos

salvando o resultado em um banco de dados (SQL Server)

O projeto pode ser dividido como: (figura abaixo)

| launcher | gadget                    | layout                                      |
|----------|---------------------------|---------------------------------------------|
| main app | documento                 | bag                                         |
|          | imagem                    | horizontal: shelf, drawer                   |
|          | vídeo etc.                | vertical: chest                             |

![](media/c6a66df256daf27eb3648ed684493f9e.jpg)

*Layout*:

Os gadgets são formatados para uma matriz (dashboard) de 4cx4r dando um total de

16 possíveis layouts (tamanhos). Para identificar o size de uma cell neste
dashboard

usa-se “style”.

| Style               |                |
|---------------------|----------------|
| horizontal (column) | Vertical (row) |
| mini                | mini           |
| small               | small          |
| large               | large          |
| big                 | big            |

![](media/b1ce885d52718db50b27de09967fbb05.jpg)

*Settings*:

\- tamanho (pixel) da coluna

\- connection string SQL Server

*Ferramentas* (tools):

| IDE                | Language          | Tools                                                                                                   | UI                                            |
|--------------------|-------------------|---------------------------------------------------------------------------------------------------------|-----------------------------------------------|
| Visual Studio 2019 | C\# 7.3           | Caliburn.Micro 3.2 (MVVM pattern)                                                                       | MaterialDesignThemes 2.5                      |
|                    | .Net 4.7.2        | XDMessaging.Lite 5                                                                                      | gong-wpf-dragdrop 1.1                         |
|                    |                   | Microsoft.EntityFrameworkCore.SqlServer 2.2                                                             |                                               |
|                    |                   | rrLibrary                                                                                               |                                               |
