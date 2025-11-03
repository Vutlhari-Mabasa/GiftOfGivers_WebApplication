"""
Load Testing Script for GiftOfGivers Web Application
Run with: python LoadTestScript.py
Requirements: pip install requests
"""

import requests
import time
from concurrent.futures import ThreadPoolExecutor, as_completed
import statistics
from datetime import datetime

BASE_URL = "http://localhost:5000"  # Update with your actual URL

class LoadTester:
    def __init__(self, base_url):
        self.base_url = base_url
        self.results = []
        
    def make_request(self, endpoint, method="GET", data=None):
        """Make a single HTTP request"""
        url = f"{self.base_url}{endpoint}"
        start_time = time.time()
        
        try:
            if method == "GET":
                response = requests.get(url, timeout=30)
            elif method == "POST":
                response = requests.post(url, json=data, timeout=30)
            else:
                response = None
                
            end_time = time.time()
            response_time = (end_time - start_time) * 1000  # Convert to ms
            
            return {
                'success': response.status_code < 400,
                'status_code': response.status_code if response else None,
                'response_time': response_time,
                'error': None
            }
        except Exception as e:
            return {
                'success': False,
                'status_code': None,
                'response_time': None,
                'error': str(e)
            }
    
    def run_load_test(self, endpoint, num_requests=100, concurrent_users=10):
        """Run load test with specified parameters"""
        print(f"\n{'='*60}")
        print(f"Load Test: {endpoint}")
        print(f"Total Requests: {num_requests}")
        print(f"Concurrent Users: {concurrent_users}")
        print(f"{'='*60}\n")
        
        results = []
        start_time = time.time()
        
        with ThreadPoolExecutor(max_workers=concurrent_users) as executor:
            futures = [
                executor.submit(self.make_request, endpoint)
                for _ in range(num_requests)
            ]
            
            for future in as_completed(futures):
                results.append(future.result())
        
        end_time = time.time()
        total_time = end_time - start_time
        
        # Calculate statistics
        successful_requests = [r for r in results if r['success']]
        response_times = [r['response_time'] for r in successful_requests if r['response_time']]
        
        print(f"Results:")
        print(f"  Total Requests: {len(results)}")
        print(f"  Successful: {len(successful_requests)}")
        print(f"  Failed: {len(results) - len(successful_requests)}")
        print(f"  Success Rate: {len(successful_requests)/len(results)*100:.2f}%")
        print(f"  Total Time: {total_time:.2f}s")
        print(f"  Requests/sec: {len(results)/total_time:.2f}")
        
        if response_times:
            print(f"\nResponse Times:")
            print(f"  Average: {statistics.mean(response_times):.2f}ms")
            print(f"  Median: {statistics.median(response_times):.2f}ms")
            if len(response_times) > 1:
                print(f"  Std Dev: {statistics.stdev(response_times):.2f}ms")
                print(f"  Min: {min(response_times):.2f}ms")
                print(f"  Max: {max(response_times):.2f}ms")
                if len(response_times) >= 10:
                    sorted_times = sorted(response_times)
                    p95_index = int(len(sorted_times) * 0.95)
                    print(f"  95th Percentile: {sorted_times[p95_index]:.2f}ms")
        
        print(f"\n{'='*60}\n")
        
        return results

def main():
    """Main execution"""
    print("\n" + "="*60)
    print("GiftOfGivers Web Application - Load Testing Suite")
    print("="*60)
    
    tester = LoadTester(BASE_URL)
    
    # Define test scenarios
    scenarios = [
        # (endpoint, num_requests, concurrent_users)
        ("/", 100, 10),                      # Home page
        ("/IncidentReports", 200, 20),       # Incident reports
        ("/ResourceTracking", 200, 20),      # Resource tracking
        ("/VolunteerTasks", 150, 15),        # Volunteer tasks
    ]
    
    all_results = []
    
    for endpoint, num_requests, concurrent_users in scenarios:
        results = tester.run_load_test(endpoint, num_requests, concurrent_users)
        all_results.extend(results)
        time.sleep(2)  # Brief pause between tests
    
    print("\n" + "="*60)
    print("LOAD TEST SUMMARY")
    print("="*60)
    
    successful = [r for r in all_results if r['success']]
    print(f"\nOverall Success Rate: {len(successful)/len(all_results)*100:.2f}%")
    print(f"Total Requests: {len(all_results)}")
    print(f"Successful: {len(successful)}")
    print(f"Failed: {len(all_results) - len(successful)}")
    
    print("\nLoad testing completed!")
    print("="*60 + "\n")

if __name__ == "__main__":
    main()


