export interface PagedResult<T> {
    count: number;
    pageNumber: number;
    pageSize: number;
    data: Array<T>;
}