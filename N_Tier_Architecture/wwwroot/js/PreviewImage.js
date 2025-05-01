$(document).ready(function () {
    //$('#chooseimage').change(function (e) {
        //debugger
        //Method 1:
        $("#previewimage").click(function () {
            $("#chooseimage").trigger('click')
        });

        //Show Image in Picture Box
        $('#chooseimage').change(function () {
            if (this.files && this.files[0]) {
                var fileReader = new FileReader();
                fileReader.readAsDataURL(this.files[0]);
                fileReader.onload = function (x) {
                    $('#previewimage').attr('src', x.target.result);
                }
            }
        }) 
        //Method 2:
        //previewimage.src = URL.createObjectURL(e.target.files[0]);

        //Method 3:
        //const [file] = chooseimage.files;
        //if (file) {
        //    previewimage.src = URL.createObjectURL(file);
        //}

        //Method 4:
        //var reader = new FileReader();
        //reader.onload = function () {
        //    var output = document.getElementById('previewimage');
        //    output.src = reader.result;
        //}
        //reader.readAsDataURL(e.target.files[0]);

        //Method 5:
        //var url = $('#chooseimage').val();
        //var ext = url.substring(url.lastIndexOf('.') +1).toLowerCase();
        //if (chooseimage.files && chooseimage.files[0] && (ext == 'gif' || ext== 'jfif' || ext == 'png' || ext == 'jpg' || ext == 'jpeg')) {
        //    var reader = new FileReader();
        //    reader.onload = function () {
        //        var output = document.getElementById('previewimage');
        //        output.src = reader.result;
        //    }
        //    reader.readAsDataURL(e.target.files[0]);
        //}

        

    //});
});