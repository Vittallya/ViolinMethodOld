function showFilePdf(fileInfo, pageCallback, finishedCallback) {

    pdfjsLib.getDocument(fileInfo).then((doc) => {
        

        for (let i = 0; i < doc._pdfInfo.numPages; i++) {
            doc.getPage(i + 1).then(page => pageCallback(page, doc, i + 1));
        }
        if (finishedCallback != undefined) {
            finishedCallback(doc, doc._pdfInfo.numPages)
        }
    });
}

function detailsPageIterator(page) {
}