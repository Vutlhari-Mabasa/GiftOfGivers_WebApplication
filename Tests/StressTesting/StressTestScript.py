"""
Stress Testing Script for GiftOfGivers Web Application
This script pushes the application beyond normal load limits
Run with: python StressTestScript.py
Requirements: pip install requests
"""

import requests
import time
from concurrent.futures import ThreadPoolExecutor, as_completed
import statistics
from datetime import datetime

BASE_URL = "http://localhost:5000"  # Update with your actual URL

class StressTester:
    def __init__(self, base_url):
        self.base_url = base_url
        
    def make_request(self, endpoint):
        """Make a single HTTP request"""
        url = f"{self.base_url}{endpoint}"
        start_time = time.time()
        
        try:
            response = requests.get(url, timeout=30)
            end_time = time.time()
            response_time = (end_time - start_time) * 1000
            
            return {
                'success': response.status_code < 400,
                'response_time': response_time,
                'status_code': response.status_code
            }
        except Exception as e:
            return {
                'success': False,
                'response_time': None,
                'status_code': None
            }
    
    def run_stress_test(self, endpoint, num_requests=1000, concurrent_users=100):
        """Run stress test with specified parameters"""
        print(f"\n{'='*60}")
        print(f"STRESS TEST: {endpoint}")
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
        successful = [r for r in results if r['success']]
        response_times = [r['response_time'] for r in successful if r['response_time']]
        
        print(f"Results:")
        print(f"  Total Requests: {len(results)}")
        print(f"  Successful: {len(successful)} ({len(successful)/len(results)*100:.2f}%)")
        print(f"  Failed: {len(results) - len(successful)}")
        print(f"  Total Time: {total_time:.2f}s")
        print(f"  Requests/sec: {len(results)/total_time:.2f}")
        
        if response_times:
            print(f"\nResponse Times (ms):")
            print(f"  Average: {statistics.mean(response_times):.2f}")
            print(f"  Median: {statistics.median(response_times):.2f}")
            print(f"  Max: {max(response_times):.2f}")
            if len(response_times) > 1:
                print(f"  Std Dev: {statistics.stdev(response_times):.2f}")
        
        print(f"{'='*60}\n")
        
        return results

def main():
    """Main execution"""
    print("\n" + "="*60)
    print("GiftOfGivers Web Application - STRESS TESTING")
    print("="*60)
    
    tester = StressTester(BASE_URL)
    
    # Define stress test scenarios
    scenarios = [
        # (endpoint, num_requests, concurrent_users)
        ("/", 2000, 200),                    # Extreme load on home page
        ("/IncidentReports", 1500, 150),     # High volume on incidents
        ("/ResourceTracking", 1500, 150),    # High volume on resources
    ]
    
    all_results = []
    
    for endpoint, num_requests, concurrent_users in scenarios:
        results = tester.run_stress_test(endpoint, num_requests, concurrent_users)
        all_results.extend(results)
        time.sleep(5)  # Longer pause between stress tests
    
    print("\n" + "="*60)
    print("STRESS TEST SUMMARY")
    print("="*60)
    
    successful = [r for r in all_results if r['success']]
    print(f"\nOverall Success Rate: {len(successful)/len(all_results)*100:.2f}%")
    print(f"Total Requests: {len(all_results)}")
    print(f"Breakdown: {len(successful)} successful, {len(all_results) - len(successful)} failed")
    
    # Performance assessment
    success_rate = len(successful)/len(all_results)*100
    if success_rate >= 99:
        print("\n✓ PASS: Application handled extreme load exceptionally well!")
    elif success_rate >= 95:
        print("\n✓ PASS: Application handled extreme load well")
    elif success_rate >= 90:
        print("\n⚠ WARNING: Application showed signs of stress")
    else:
        print("\n✗ FAIL: Application failed under extreme load")
    
    print("\nStress testing completed!")
    print("="*60 + "\n")

if __name__ == "__main__":
    main()


