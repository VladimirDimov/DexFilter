import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  ColDef,
  GridOptions,
  GridReadyEvent,
  IServerSideDatasource,
} from 'ag-grid-community';
import 'ag-grid-enterprise';
import * as moment from 'moment';

@Component({
  selector: 'app-home',
  templateUrl: './basic-example.component.html',
})
export class BasicExampleComponent {
  public columnDefs: ColDef[] = [
    {
      field: 'firstName',
      headerName: 'First Name',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'lastName',
      headerName: 'Last Name',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'grade',
      headerName: 'Grade',
      filter: 'number',
      sortable: true,
    },
    {
      field: 'birthDate',
      headerName: 'Date Of Birth',
      filter: 'date',
      sortable: true,
      valueFormatter: (p) => this.dateFormatter(p.data.birthDate),
    },
    {
      field: 'address.city.name',
      headerName: 'City',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'address.street',
      headerName: 'Street',
      filter: 'text',
      sortable: true,
    },
    {
      field: 'address.postalCode',
      headerName: 'Postal Code',
      filter: 'text',
      sortable: true,
    }
  ];

  public defaultColDef: ColDef = {
    flex: 1,
    minWidth: 100,
  };
  public rowModelType = 'serverSide';
  public rowData!: any[];
  gridOptions: GridOptions = {
    columnDefs: this.columnDefs,
  };

  constructor(private http: HttpClient) { }

  ngOnInit(): void { }

  onGridReady(params: GridReadyEvent) {
    var datasource = this.createServerSideDatasource();
    // register the datasource with the grid
    params.api!.setServerSideDatasource(datasource);
  }

  createServerSideDatasource(): IServerSideDatasource {
    const internalThis = this;
    return {
      getRows: function (params) {
        internalThis.http
          .post<any[]>('http://localhost:5238/students/basic-example', params.request)
          .subscribe(
            (data: any) => {
              params.success({
                rowData: data.rowData,
                rowCount: data.rowCount,
              });
            },
            () => params.fail()
          );
      },
    };
  }

  resetFilters(): void {
    this.gridOptions.api?.setFilterModel(null);
  }

  dateFormatter(date: string) {
    if (!date) return '-';
    return moment(date).format('DD-MM-YYYY');
  }
}
