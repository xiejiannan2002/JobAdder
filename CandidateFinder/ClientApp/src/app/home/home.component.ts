import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  public baseUrl: string;
  public http: HttpClient;
  public currentJobs: Job[] = null;
  public currentPage: number = 0;
  public pageCount: number = 6;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }

  ngOnInit() {
      this.getAllJobs();
  }

  public getAllJobs() {
    this.http.get<any>(this.baseUrl + 'home/get/')
      .subscribe(result => {
        //console.log(result);
        this.currentJobs = result;
      }, error => console.error(error));
  }

  public startIndex(): number {
    return this.currentPage * this.pageCount;
  }

  public endIndex(): number {
    return (this.currentPage + 1) * this.pageCount;
  }

  public Up() {
    if (this.currentPage >= 1)
      this.currentPage--;
    this.startIndex();
    this.endIndex();
  }

  public Down() {
    if ((this.currentPage + 1) * this.pageCount < this.currentJobs.length)
      this.currentPage++;
    this.startIndex();
    this.endIndex();
  }
}

interface  Candidate {
  name: string;
  skillTags: string;
}

interface Job {
  name: string;
  company: string;
  skills: string;
  candidate: Candidate
}

