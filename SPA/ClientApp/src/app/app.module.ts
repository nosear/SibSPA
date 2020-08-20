import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent, GroupByPipe } from './app.component';
import { DataService } from './data.service';

@NgModule({
  imports: [BrowserModule, FormsModule, HttpClientModule],
  declarations: [
    AppComponent,
    GroupByPipe
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
