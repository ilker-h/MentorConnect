import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  user: User | undefined;
  userParams: UserParams | undefined;

  // inject HttpClient. // A service can be injected into another service without causing an issue, but if you also do it the other way around,
  // it becomes a circular reference and won't work.
  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    });
   }

   getUserParams() {
    return this.userParams;
   }

   setUserParams(params: UserParams) {
    this.userParams = params;
   }

   resetUserParams() {
    if (this.user) {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
   }

  getMembers(userParams: UserParams) {
    // these key-value pairs of the memberCache map look like {"0-50-1-5-lastActive-Mentee": PaginatedResult},
    // based on the query string sent to the API. The PaginatedResult object contains the users on that page.
    const response = this.memberCache.get(Object.values(userParams).join('-'));

    if (response) return of(response);

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minYearsOfCareerExperience', userParams.minYearsOfCareerExperience);
    params = params.append('maxYearsOfCareerExperience', userParams.maxYearsOfCareerExperience);
    params = params.append('mentorOrMentee', userParams.mentorOrMentee);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    );

  }

  getMember(username: string) {
    const member = [...this.memberCache.values()]
      .reduce((prevValueArr, currentValueElem) => prevValueArr.concat(currentValueElem.result), [])
      .find((member: Member) => member.userName === username);

      if (member) return of(member);

    // if the user is not in the local cache, this goes to the API to retrieve it
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        // the first spread operator ... will spread the properties of the member (e.g. "Bio", "CareerInterests"")
        // the second spread operator ... will spread the elements inside the member
        this.members[index] = { ...this.members[index], ...member }
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {})
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addConnectionRequest(username: string) {
    return this.http.post(this.baseUrl + 'connectionrequests/' + username, {});
  }

  getConnectionRequests(predicate: string, pageNumber: number, pageSize: number) {
    let params = this.getPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'connectionrequests', params);
  }

  // <T> makes the method generic so that it can be used for filtering different types of things, not just Member[]
  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;

    return this.http.get<T>(url, { observe: 'response', params }).pipe(
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

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams(); // HttpParams() is a utility/class provided by Angular that allows for setting query string parameters along with the Http request

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize); // pageSize means items shown per page

    return params;
  }

}
