import { User } from './user';

export class UserParams {
  gender: string;
  minAge = 1;
  maxAge = 70;
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive'

  constructor(User: User | null) {
    this.gender = User?.gender === 'female' ? 'female' : 'male';
  }
}
