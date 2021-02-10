import React, { Component } from 'react';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { ColumnDirective, ColumnsDirective, GridComponent } from '@syncfusion/ej2-react-grids';
import { Inject, Toolbar } from '@syncfusion/ej2-react-grids';
import { isNullOrUndefined, Internationalization } from '@syncfusion/ej2-base';

export class Home extends Component {

    constructor(props) {
        super(props);
        this.dataManager = new DataManager({
            adaptor: new UrlAdaptor(),
            url: "Home/UrlDatasource"
        });
        this.toolbar = ['ExcelExport', 'PdfExport'];
        this.toolbarClick = (args) => {
            if (this.grid && args.item.id === 'grid_excelexport') {
                this.exportGrid('Home/ExcelExport');
            }
            if (this.grid && args.item.id === 'grid_pdfexport') {
                this.exportGrid('Home/PdfExport');
            }

        }
        this.exportGrid = (url) => {
            var grid = this.grid;
            var gridModel = JSON.parse(this.grid.addOnPersist(['allowGrouping', 'allowPaging', 'pageSettings', 'sortSettings', 'allowPdfExport', 'allowExcelExport', 'aggregates',
                'filterSettings', 'groupSettings', 'columns', 'locale', 'searchSettings']));
            gridModel.columns.forEach((e) => {
                let column = grid.getColumnByUid(e.uid);
                if (column) {
                    e.headerText = column.headerText;
                    if (e.format) {
                        var format = typeof (e.format) === 'object' ? e.format.format : e.format;
                        e.format = this.getNumberFormat(format, e.type);
                    }
                }
                if (e.columns) {
                    this.setHeaderText(e.columns);
                }
            });
            var form = this.grid.createElement('form', { id: 'ExportForm', styles: 'display:none;' });
            var gridInput = this.grid.createElement('input', { id: 'gridInput', attrs: { name: "gridModel" } });
            gridInput.value = JSON.stringify(gridModel);
            form.method = "POST";
            form.action = url;
            form.appendChild(gridInput);
            document.body.appendChild(form);
            form.submit();
            form.remove();
        }

        this.setHeaderText = (columns) => {
            for (var i = 0; i < columns.length; i++) {
                let column = this.grid.getColumnByUid(columns[i].uid);
                columns[i].headerText = column.headerText;
                if (columns[i].format) {
                    var e = columns[i];
                    var format = typeof (e.format) === 'object' ? e.format.format : e.format;
                    columns[i].format = this.getNumberFormat(format, columns[i].type);
                }
                if (columns[i].columns) {
                    this.setHeaderText(columns[i].columns);
                }
            }
        }

        this.getNumberFormat = (numberFormat, type) => {
            var format;
            var intl = new Internationalization();
            if (type === 'number') {
                try {
                    format = intl.getNumberPattern({ format: numberFormat, currency: this.currency, useGrouping: true }, true);
                } catch (error) {
                    format = numberFormat;
                }
            } else if (type === 'date' || type === 'time' || type === 'datetime') {
                try {
                    format = intl.getDatePattern({ skeleton: numberFormat, type: type }, false);
                } catch (error) {
                    try {
                        format = intl.getDatePattern({ format: numberFormat, type: type }, false);
                    } catch (error) {
                        format = numberFormat;
                    }
                }
            } else {
                format = numberFormat;
            }
            if (type !== 'number') {
                let patternRegex = /G|H|c|'| a|yy|y|EEEE|E/g;
                let mtch = { 'G': '', 'H': 'h', 'c': 'd', '\'': '"', ' a': ' AM/PM', 'yy': 'yy', 'y': 'yyyy', 'EEEE': 'dddd', 'E': 'ddd' };
                format = format.replace(patternRegex, (pattern) => {
                    return (mtch)[pattern];
                });
            }
            return format;
        }
    }

    render() {
        this.toolbarClick = this.toolbarClick.bind(this);
        return (
            <div>
                <GridComponent id='grid' dataSource={this.dataManager} height={270} toolbar={this.toolbar} toolbarClick={this.toolbarClick} ref={g => this.grid = g}>
                    <ColumnsDirective>
                        <ColumnDirective field='OrderID' headerText='Order ID' width='120' textAlign='Right' />
                        <ColumnDirective field='CustomerID' headerText='Customer ID' width='150' />
                        <ColumnDirective field='Freight' format="C2" width='100' textAlign='Right' />
                        <ColumnDirective field='ShipCity' headerText='Ship City' width='150' />
                        <ColumnDirective field='ShipName' headerText='Ship Name' width='150' />
                    </ColumnsDirective>
                    <Inject services={[Toolbar]} />
                </GridComponent>
            </div>
        );
    }
}
