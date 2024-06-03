export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

// when the reponse gets back from the API, the below info will be fished out from the response's header
export class PaginatedResult<T> {
  result?: T;
  pagination?: Pagination;
}
