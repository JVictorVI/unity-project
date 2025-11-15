# O Segredo da Biblioteca

Repositório principal do projeto Unity

Há uma segunda branch chamada 'projetos_antigos', que contém as versões dos projetos anteriores funcionando, como backup.

## Importante

* Caso tenham receio de enviar para a branch principal e comprometer o projeto, criem uma branch paralela, mas é interessante que já seja na principal para evitar retrabalho.
* Recomendo utilizar o GitHub Desktop, facilita versionar com projetos Unity.

## Tutorial

[COMO USAR O GITHUB COM A UNITY](https://www.youtube.com/watch?v=hhAV89FEdCM)

## Como Exportar e Importar um Projeto Unity

### Exportar um projeto para outro

1. Abra o projeto original no Unity.
2. No *Project Window*, selecione as pastas ou arquivos que deseja exportar.
3. Clique com o botão direito e selecione **Export Package**.
4. Marque a opção **Include Dependencies** para garantir que todos os arquivos necessários sejam incluídos.
5. Clique em **Export** e salve o arquivo `.unitypackage`.

### Importar em outro projeto

1. Abra o projeto de destino no Unity.
2. Vá em **Assets > Import Package > Custom Package**.
3. Selecione o `.unitypackage` exportado.
4. Na janela de importação, confira os arquivos que serão adicionados.
5. Clique em **Import**.