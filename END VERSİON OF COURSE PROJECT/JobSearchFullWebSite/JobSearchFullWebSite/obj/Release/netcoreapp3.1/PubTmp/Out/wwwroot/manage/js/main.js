$(document).ready(function () {


    $(document).on("click", ".delete-btn", function (e) {
        e.preventDefault();

        console.log("testing")

        var url = $(this).attr("href")
        console.log(url);

        Swal.fire({
            title: 'Əminsiniz?',
            text: "Geri qaytara bilməyəcəksiz",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Bəli',
            cancelButtonText: 'Xeyr'
        }).then((result) => {

            if (result.isConfirmed) {
                console.log(url);
                fetch(url)
                    .then(response => response.json())
                    .then(data => console.log(data));
                window.location.reload();
                Swal.fire(
                    'Silindi',
                    '',
                    'success'
                )

            }


        })
    })})