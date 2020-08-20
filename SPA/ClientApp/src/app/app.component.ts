import { Component, OnInit, Pipe, ViewChild, PipeTransform } from '@angular/core';
import { DataService } from './data.service';
import { saveAs as importedSaveAs } from "file-saver";
import bsCustomFileInput from 'bs-custom-file-input';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
})

export class AppComponent implements OnInit {
  public files: any;

  constructor(private service: DataService) { }
  @ViewChild("fileInput", { static: false }) fileInput;
  addFile(): void {
    let fi = this.fileInput.nativeElement;
    if (fi.files && fi.files[0]) {
      for (var i = 0; i < fi.files.length; i++) {
        let fileToUpload = fi.files[i];
        this.service
          .upload(fileToUpload)
          .subscribe(res => {
            console.log(res);
          });
      }
    }
  }

  ngOnInit() {
    bsCustomFileInput.init()
    this.getFiles()
  }
  
  private getFiles() {
    this.service.getFiles().subscribe(
      data => {
        this.files = data;
      }
    );
  }

  removeFile(file) {

    this.service.deleteFile(file.id).subscribe(data => {
      this.files = this.files.filter(item => item.id !== file.id);
    }, error => console.error(error));
  }

  public download(file) {
    this.service.downloadFile(file.id).subscribe(blob => {
      this.files = this.files.filter(item => item.id !== file.id);
      importedSaveAs(blob, file.name);
    }
    );
  }
}
@Pipe({ name: 'groupBy' })
export class GroupByPipe implements PipeTransform {
  transform(value: Array<any>, field: string): Array<any> {
    const groupedObj = value.reduce((prev, cur) => {
      if (!prev[cur[field]]) {
        prev[cur[field]] = [cur];
      } else {
        prev[cur[field]].push(cur);
      }
      return prev;
    }, {});
    return Object.keys(groupedObj).map(key => ({ key, value: groupedObj[key] }));
  }
}
