$(document).ready(function() {
    // Setup - add a text input to each footer cell
    $('#example thead tr').clone(true).appendTo( '#example thead' );
    $('#example thead tr:eq(1) th').each( function (i) {
        var title = $(this).text();
        $(this).html( '<input type="text" placeholder="Search '+title+'" />' );
 
        $( 'input', this ).on( 'keyup change', function () {
            if ( table.column(i).search() !== this.value ) {
                table
                    .column(i)
                    .search( this.value )
                    .draw();
            }
        } );
    } );
 
    var table = $('#example').DataTable( {
        orderCellsTop: true,
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [ {
          extend: 'excel',
          text: 'Export to Excel'
        }
        ]
    });


    $('#example1 thead tr').clone(true).appendTo('#example1 thead');
    $('#example1 thead tr:eq(1) th').each(function (i) {
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

    var table = $('#example1').DataTable({
        orderCellsTop: true,
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [{
            extend: 'excel',
            text: 'Export to Excel'
        }
        ]
    });

    var table = $('#gridMapping').DataTable({
        orderCellsTop: true,
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [{
            extend: 'excel',
            text: 'Export to Excel'
        }
        ]
    });
    

    var table = $('#example3').DataTable({
        orderCellsTop: false,
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [{
            extend: 'excel',
            text: 'Export to Excel'
        }
        ]
    });




    $('#example2 thead tr').clone(true).appendTo('#example2 thead');
    $('#example2 thead tr:eq(1) th').each(function (i) {
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

    var table = $('#example2').DataTable({
        orderCellsTop: true,
        fixedHeader: true,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [{
            extend: 'excel',
            text: 'Export to Excel'
        }
        ],
        "columns": [
    { "width": "150px" },
    { "width": "150px" },
    { "width": "100px" }
        ]
    });

    $("#contacts-dt").DataTable({
        orderCellsTop: true,
        fixedHeader: true,
        scrollX: true,
        "searching": false,
        "lengthChange": false
    });

   
    $('#library-dt').DataTable({
        orderCellsTop: false,
        fixedHeader: false,
        scrollX: true,
        "bAutoWidth": false,
        "bLengthChange": false,
        "targets": 'no-sort',
        "bSort": false
    });



} );