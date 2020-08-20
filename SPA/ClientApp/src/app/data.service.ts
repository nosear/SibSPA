import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class DataService {
  private baseApiUrl: string;
  private deleteApiUrl: string;
  private downloadApiUrl: string;
  constructor(private http: HttpClient) {

    this.baseApiUrl = "api/modfiles";
    this.deleteApiUrl = "api/modfiles/delete";
    this.downloadApiUrl = "api/modfiles/download";
  }
  public upload(fileToUpload: any) {
    let input = new FormData();
    input.append("file", fileToUpload);

    return this.http
      .post(this.baseApiUrl, input);
  }
  public deleteFile(id: number) {
    return this.http.delete(this.deleteApiUrl + '/' + id);
  }
  downloadFile(id: number): Observable<Blob> {
    return this.http.get(this.downloadApiUrl + '/' + id, { responseType: 'blob' });
  }
  public getFiles() {
    return this.http.get(this.baseApiUrl);
  }
}
