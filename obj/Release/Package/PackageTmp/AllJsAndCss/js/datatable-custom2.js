$(document).ready(function() {
    // Setup - add a text input to each footer cell
    //$('#example thead tr').clone(true).appendTo( '#example thead' );
    //$('#example thead tr:eq(1) th').each( function (i) {
    //    var title = $(this).text();
    //    $(this).html( '<input type="text" placeholder="Search '+title+'" />' );
 
    //    $( 'input', this ).on( 'keyup change', function () {
    //        if ( table.column(i).search() !== this.value ) {
    //            table
    //                .column(i)
    //                .search( this.value )
    //                .draw();
    //        }
    //    } );
    //} );  

    $('#example thead tr').clone(true).appendTo('#example thead');
    $('#example thead tr:eq(1) th').each(function (i) {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Search ' + title + '" />');

        $('input', this).on('keyup change', function () {
            if (table.column(i).search() !== this.value) {
                table
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
    });
 
    var table = $('#example').DataTable({
        //orderCellsTop: false,
        
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [{
            extend: 'excel',
            text: 'Export to Excel'
        },
        ],
        "ordering": false
    });

    var table = $('#example1').DataTable({
        //orderCellsTop: false,

        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        //buttons: [{
        //    extend: 'excel',
        //    text: 'Export to Excel'
        //},
        //],
        "ordering": false
    });



//    $('#gridMapping thead tr').clone(true).appendTo('#gridMapping thead');
//    $('#gridMapping thead tr:eq(1) th').each(function (i) {
//        var title = $(this).text();
//        $(this).html('<input type="text" placeholder="Search ' + title + '" />');

//        $('input', this).on('keyup change', function () {
//            if (table.column(i).search() !== this.value) {
//                table
//                    .column(i)
//                    .search(this.value)
//                    .draw();
//            }
//        });
//    });

//    var table = $('#gridMapping').DataTable({
//        //orderCellsTop: false,

//        fixedHeader: true,
//        scrollX: true,
//        dom: 'Bfrtip',
//        buttons: [{
//            extend: 'excel',
//            text: 'Export to Excel'
//        },
//        ],
//        "ordering": false
//    });

} );