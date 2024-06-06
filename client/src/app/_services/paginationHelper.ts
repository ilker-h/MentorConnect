import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

  // <T> makes the method generic so that it can be used for filtering different types of things, not just Member[]
  export function getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;

    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      })
    );

    // The members [] array will be stored in this service since services persist even when components are destroyed.
    // If members were pulled from the API already, there's no need to call the API to get them again.
    // if (this.members.length > 0) return of(this.members); // must return an observable, that's why of() is used instead of just this.members
    // return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
    //   map(members => {
    //     this.members = members;
    //     return members;
    //   })
    // )
  }

  export function getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams(); // HttpParams() is a utility/class provided by Angular that allows for setting query string parameters along with the Http request

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize); // pageSize means items shown per page

    return params;
  }
